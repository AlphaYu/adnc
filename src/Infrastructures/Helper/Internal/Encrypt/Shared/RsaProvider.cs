namespace Adnc.Infra.Helper.Internal.Encrypt.Shared;

/// <summary>
/// RSA provider
/// https://github.com/xiangyuecn/RSA-csharp
/// </summary>
internal sealed class RsaProvider
{
#pragma warning disable SYSLIB1045 // “GeneratedRegexAttribute”。
    private static readonly Regex _pEMCode = new(@"--+.+?--+|\s+");
#pragma warning restore SYSLIB1045 // “GeneratedRegexAttribute”。
    private static readonly byte[] _seqOID = [0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00];
    private static readonly byte[] _ver = [0x02, 0x01, 0x00];

    /// <summary>
    /// Convert pem to rsa，support PKCS#1、PKCS#8 
    /// </summary>
    internal static RSA FromPem(string pem)
    {
        //var rsaParams = new CspParameters();
        //rsaParams.Flags = CspProviderFlags.UseMachineKeyStore;
        //var rsa = new RSACryptoServiceProvider(rsaParams);

        var rsa = RSA.Create();

        var param = new RSAParameters();

        var base64 = _pEMCode.Replace(pem, "");
        var data = Convert.FromBase64String(base64) ?? throw new InvalidDataException("Pem content invalid ");
        var idx = 0;

        //read  length
        int readLen(byte first)
        {
            if (data[idx] == first)
            {
                idx++;
                if (data[idx] == 0x81)
                {
                    idx++;
                    return data[idx++];
                }
                else if (data[idx] == 0x82)
                {
                    idx++;
                    return (data[idx++] << 8) + data[idx++];
                }
                else if (data[idx] < 0x80)
                {
                    return data[idx++];
                }
            }
            throw new InvalidDataException("Not found any content in pem file");
        }
        //read module length
        byte[] readBlock()
        {
            var len = readLen(0x02);
            if (data[idx] == 0x00)
            {
                idx++;
                len--;
            }
            var val = data.Sub(idx, len);
            idx += len;
            return val;
        }

        bool eq(byte[] byts)
        {
            for (var i = 0; i < byts.Length; i++, idx++)
            {
                if (idx >= data.Length)
                {
                    return false;
                }
                if (byts[i] != data[idx])
                {
                    return false;
                }
            }
            return true;
        }

        if (pem.Contains("PUBLIC KEY"))
        {
            /****Use public key****/
            readLen(0x30);
            if (!eq(_seqOID))
            {
                throw new InvalidDataException("Unknown pem format");
            }

            readLen(0x03);
            idx++;
            readLen(0x30);

            //Modulus
            param.Modulus = readBlock();

            //Exponent
            param.Exponent = readBlock();
        }
        else if (pem.Contains("PRIVATE KEY"))
        {
            /****Use private key****/
            readLen(0x30);

            //Read version
            if (!eq(_ver))
            {
                throw new InvalidDataException("Unknown pem version");
            }

            //Check PKCS8
            var idx2 = idx;
            if (eq(_seqOID))
            {
                //Read one byte
                readLen(0x04);

                readLen(0x30);

                //Read version
                if (!eq(_ver))
                {
                    throw new InvalidDataException("Pem version invalid");
                }
            }
            else
            {
                idx = idx2;
            }

            //Reda data
            param.Modulus = readBlock();
            param.Exponent = readBlock();
            param.D = readBlock();
            param.P = readBlock();
            param.Q = readBlock();
            param.DP = readBlock();
            param.DQ = readBlock();
            param.InverseQ = readBlock();
        }
        else
        {
            throw new InvalidDataException("pem need 'BEGIN' and  'END'");
        }

        rsa.ImportParameters(param);
        return rsa;
    }

    /// <summary>
    /// Converter Rsa to pem ,
    /// </summary>
    /// <param name="rsa"><see cref="RSACryptoServiceProvider"/></param>
    /// <param name="includePrivateParameters">if false only return publick key</param>
    /// <param name="isPKCS8">default is false,if true return PKCS#8 pem else return PKCS#1 pem </param>
    /// <returns></returns>
    internal static string ToPem(RSA rsa, bool includePrivateParameters, bool isPKCS8 = false)
    {
        var ms = new MemoryStream();

        void writeLenByte(int len)
        {
            if (len < 0x80)
            {
                ms.WriteByte((byte)len);
            }
            else if (len <= 0xff)
            {
                ms.WriteByte(0x81);
                ms.WriteByte((byte)len);
            }
            else
            {
                ms.WriteByte(0x82);
                ms.WriteByte((byte)(len >> 8 & 0xff));
                ms.WriteByte((byte)(len & 0xff));
            }
        }
        //write moudle data
        void writeBlock(byte[] byts)
        {
            var addZero = byts[0] >> 4 >= 0x8;
            ms.WriteByte(0x02);
            var len = byts.Length + (addZero ? 1 : 0);
            writeLenByte(len);

            if (addZero)
            {
                ms.WriteByte(0x00);
            }
            ms.Write(byts, 0, byts.Length);
        }

        byte[] writeLen(int index, byte[] byts)
        {
            var len = byts.Length - index;

            ms.SetLength(0);
            ms.Write(byts, 0, index);
            writeLenByte(len);
            ms.Write(byts, index, len);

            return ms.ToArray();
        }

        if (!includePrivateParameters)
        {
            /****Create public key****/
            var param = rsa.ExportParameters(false);

            ms.WriteByte(0x30);
            var index1 = (int)ms.Length;

            // Encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            ms.WriteAll(_seqOID);

            //Start with 0x00 
            ms.WriteByte(0x03);
            var index2 = (int)ms.Length;
            ms.WriteByte(0x00);

            //Content length
            ms.WriteByte(0x30);
            var index3 = (int)ms.Length;

            //Write Modulus
            writeBlock(param.Modulus ?? throw new InvalidDataException(nameof(param.Modulus)));

            //Write Exponent
            writeBlock(param.Exponent ?? throw new InvalidDataException(nameof(param.Exponent)));

            var bytes = ms.ToArray();

            bytes = writeLen(index3, bytes);
            bytes = writeLen(index2, bytes);
            bytes = writeLen(index1, bytes);

            return "-----BEGIN PUBLIC KEY-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END PUBLIC KEY-----";
        }
        else
        {
            /****Create private key****/
            var param = rsa.ExportParameters(true);

            //Write total length
            ms.WriteByte(0x30);
            var index1 = (int)ms.Length;

            //Write version
            ms.WriteAll(_ver);

            //PKCS8 
            int index2 = -1, index3 = -1;
            if (isPKCS8)
            {
                ms.WriteAll(_seqOID);

                ms.WriteByte(0x04);
                index2 = (int)ms.Length;

                ms.WriteByte(0x30);
                index3 = (int)ms.Length;

                ms.WriteAll(_ver);
            }

            //Write data
            writeBlock(param.Modulus ?? throw new InvalidDataException(nameof(param.Modulus)));
            writeBlock(param.Exponent ?? throw new InvalidDataException(nameof(param.Exponent)));
            writeBlock(param.D ?? throw new InvalidDataException(nameof(param.D)));
            writeBlock(param.P ?? throw new InvalidDataException(nameof(param.P)));
            writeBlock(param.Q ?? throw new InvalidDataException(nameof(param.Q)));
            writeBlock(param.DP ?? throw new InvalidDataException(nameof(param.DP)));
            writeBlock(param.DQ ?? throw new InvalidDataException(nameof(param.DQ)));
            writeBlock(param.InverseQ ?? throw new InvalidDataException(nameof(param.InverseQ)));

            var bytes = ms.ToArray();

            if (index2 != -1)
            {
                bytes = writeLen(index3, bytes);
                bytes = writeLen(index2, bytes);
            }
            bytes = writeLen(index1, bytes);

            var flag = " PRIVATE KEY";
            if (!isPKCS8)
            {
                flag = " RSA" + flag;
            }
            return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END" + flag + "-----";
        }
    }

    /// <summary>
    /// Text break method
    /// </summary>
    private static string TextBreak(string text, int line)
    {
        var idx = 0;
        var len = text.Length;
        var str = new StringBuilder();
        while (idx < len)
        {
            if (idx > 0)
            {
                str.Append('\n');
            }
            if (idx + line >= len)
            {
                str.Append(text.AsSpan(idx));
            }
            else
            {
                str.Append(text.AsSpan(idx, line));
            }
            idx += line;
        }
        return str.ToString();
    }
}
