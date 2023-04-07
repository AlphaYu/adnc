using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// Create des key
    /// </summary>
    /// <returns></returns>
    public string CreateDesKey()
    {
        return GetRandomStr(24);
    }

    /// <summary>
    /// Create des iv
    /// </summary>
    /// <returns></returns>
    public string CreateDesIv()
    {
        return GetRandomStr(8);
    }

    /// <summary>
    /// DES encrypt
    /// </summary>
    /// <param name="data">Raw data</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <returns>Encrypted string</returns>
    public string DESEncrypt(string data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        byte[] plainBytes = Encoding.UTF8.GetBytes(data);
        var encryptBytes = DESEncrypt(plainBytes, key, CipherMode.ECB);

        if (encryptBytes == null)
        {
            return string.Empty;
        }
        return Convert.ToBase64String(encryptBytes);
    }

    /// <summary>
    /// DES encrypt
    /// </summary>
    /// <param name="data">Raw data byte array</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <returns>Encrypted byte array</returns>
    public byte[] DESEncrypt(byte[] data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        return DESEncrypt(data, key, CipherMode.ECB);
    }


    /// <summary>
    /// DES encrypt
    /// </summary>
    /// <param name="data">Raw data byte array</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <param name="vector">IV,requires 8 bits</param>
    /// <returns>Encrypted byte array</returns>
    public byte[] DESEncrypt(byte[] data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));
        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 8, nameof(vector));

        return DESEncrypt(data, key, CipherMode.CBC, vector);
    }

    /// <summary>
    /// DES encrypt
    /// </summary>
    /// <param name="data">Raw data</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <param name="cipherMode"><see cref="CipherMode"/></param>
    /// <param name="paddingMode"><see cref="PaddingMode"/> default is PKCS7</param>
    /// <param name="vector">IV,requires 8 bits</param>
    /// <returns>Encrypted byte array</returns>
    private byte[] DESEncrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        using (MemoryStream Memory = new MemoryStream())
        {
            using (TripleDES des = TripleDES.Create())
            {
                byte[] plainBytes = data;
                byte[] bKey = new byte[24];
                Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                des.Mode = cipherMode;
                des.Padding = paddingMode;
                des.Key = bKey;

                if (cipherMode == CipherMode.CBC)
                {
                    byte[] bVector = new byte[8];
                    Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
                    des.IV = bVector;
                }

                using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    try
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Memory.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("DESEncrypt", ex);
                    }
                }
            }
        }
    }

    /// <summary>
    /// DES decrypt
    /// </summary>
    /// <param name="data">Encrypted data</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <returns>Decrypted string</returns>
    public string DESDecrypt(string data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        byte[] encryptedBytes = Convert.FromBase64String(data);
        byte[] bytes = DESDecrypt(encryptedBytes, key, CipherMode.ECB);

        if (bytes == null)
        {
            return string.Empty;
        }
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// DES decrypt
    /// </summary>
    /// <param name="data">Encrypted data byte array</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <returns>Decrypted string</returns>
    public byte[] DESDecrypt(byte[] data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        return DESDecrypt(data, key, CipherMode.ECB);
    }

    /// <summary>
    /// DES encrypt
    /// </summary>
    /// <param name="data">Raw data byte array</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <param name="vector">IV,requires 8 bits</param>
    /// <returns>Encrypted byte array</returns>
    public byte[] DESDecrypt(byte[] data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));
        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 8, nameof(vector));

        return DESDecrypt(data, key, CipherMode.CBC, vector);
    }

    /// <summary>
    /// DES decrypt
    /// </summary>
    /// <param name="data">Encrypted data</param>
    /// <param name="key">Key, requires 24 bits</param>
    /// <param name="cipherMode"><see cref="CipherMode"/></param>
    /// <param name="paddingMode"><see cref="PaddingMode"/> default is PKCS7</param>
    /// <returns>Decrypted byte array</returns>
    private byte[] DESDecrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 24, nameof(key));

        byte[] encryptedBytes = data;
        byte[] bKey = new byte[24];
        Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

        using (MemoryStream Memory = new MemoryStream(encryptedBytes))
        {
            using (TripleDES des = TripleDES.Create())
            {
                des.Mode = cipherMode;
                des.Padding = paddingMode;
                des.Key = bKey;

                if (cipherMode == CipherMode.CBC)
                {
                    byte[] bVector = new byte[8];
                    Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
                    des.IV = bVector;
                }

                using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    try
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return ret;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("DESDecrypt", ex);
                    }
                }
            }
        }
    }
}
