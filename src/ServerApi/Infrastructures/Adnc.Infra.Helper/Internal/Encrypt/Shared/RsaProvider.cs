namespace Adnc.Infra.Helper.Internal.Encrypt.Shared;

/// <summary>
/// RSA provider
/// https://github.com/xiangyuecn/RSA-csharp
/// </summary>
internal class RsaProvider
{
    static private Regex _PEMCode = new Regex(@"--+.+?--+|\s+");
    static private byte[] _SeqOID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
    static private byte[] _Ver = new byte[] { 0x02, 0x01, 0x00 };

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

        var base64 = _PEMCode.Replace(pem, "");
        var data = Convert.FromBase64String(base64);
        if (data == null)
        {
            throw new Exception("Pem content invalid ");
        }
        var idx = 0;

        //read  length
        Func<byte, int> readLen = (first) =>
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
            throw new Exception("Not found any content in pem file");
        };
        //read module length
        Func<byte[]> readBlock = () =>
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
        };

        Func<byte[], bool> eq = (byts) =>
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
        };

        if (pem.Contains("PUBLIC KEY"))
        {
            /****Use public key****/
            readLen(0x30);
            if (!eq(_SeqOID))
            {
                throw new Exception("Unknown pem format");
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
            if (!eq(_Ver))
            {
                throw new Exception("Unknown pem version");
            }

            //Check PKCS8
            var idx2 = idx;
            if (eq(_SeqOID))
            {
                //Read one byte
                readLen(0x04);

                readLen(0x30);

                //Read version
                if (!eq(_Ver))
                {
                    throw new Exception("Pem version invalid");
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
            throw new Exception("pem need 'BEGIN' and  'END'");
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

        Action<int> writeLenByte = (len) =>
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
        };
        //write moudle data
        Action<byte[]> writeBlock = (byts) =>
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
        };

        Func<int, byte[], byte[]> writeLen = (index, byts) =>
        {
            var len = byts.Length - index;

            ms.SetLength(0);
            ms.Write(byts, 0, index);
            writeLenByte(len);
            ms.Write(byts, index, len);

            return ms.ToArray();
        };


        if (!includePrivateParameters)
        {
            /****Create public key****/
            var param = rsa.ExportParameters(false);

            ms.WriteByte(0x30);
            var index1 = (int)ms.Length;

            // Encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            ms.WriteAll(_SeqOID);

            //Start with 0x00 
            ms.WriteByte(0x03);
            var index2 = (int)ms.Length;
            ms.WriteByte(0x00);

            //Content length
            ms.WriteByte(0x30);
            var index3 = (int)ms.Length;

            //Write Modulus
            writeBlock(param.Modulus ?? throw new NullReferenceException(nameof(param.Modulus)));

            //Write Exponent
            writeBlock(param.Exponent ?? throw new NullReferenceException(nameof(param.Exponent)));

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
            int index1 = (int)ms.Length;

            //Write version
            ms.WriteAll(_Ver);

            //PKCS8 
            int index2 = -1, index3 = -1;
            if (isPKCS8)
            {
                ms.WriteAll(_SeqOID);

                ms.WriteByte(0x04);
                index2 = (int)ms.Length;

                ms.WriteByte(0x30);
                index3 = (int)ms.Length;

                ms.WriteAll(_Ver);
            }

            //Write data
            writeBlock(param.Modulus ?? throw new NullReferenceException(nameof(param.Modulus)));
            writeBlock(param.Exponent ?? throw new NullReferenceException(nameof(param.Exponent)));
            writeBlock(param.D ?? throw new NullReferenceException(nameof(param.D)));
            writeBlock(param.P ?? throw new NullReferenceException(nameof(param.P)));
            writeBlock(param.Q ?? throw new NullReferenceException(nameof(param.Q)));
            writeBlock(param.DP ?? throw new NullReferenceException(nameof(param.DP)));
            writeBlock(param.DQ ?? throw new NullReferenceException(nameof(param.DQ)));
            writeBlock(param.InverseQ ?? throw new NullReferenceException(nameof(param.InverseQ)));

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
                str.Append(text.Substring(idx));
            }
            else
            {
                str.Append(text.Substring(idx, line));
            }
            idx += line;
        }
        return str.ToString();
    }
}
