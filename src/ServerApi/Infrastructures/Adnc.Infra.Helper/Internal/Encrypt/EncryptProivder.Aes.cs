using Adnc.Infra.Core.Guard;
using Adnc.Infra.Helper.Internal.Encrypt.Shared;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /*
    AES: 16-bit key=128 bits, 24-bit key=192 bits, 32-bit key=256 bits, IV is 16 bits
    */
    /// <summary>
    /// Create ase key
    /// </summary>
    /// <returns></returns>
    public AESKey CreateAesKey()
    {
        return new AESKey()
        {
            Key = GetRandomStr(32),
            IV = GetRandomStr(16)
        };
    }

    /// <summary>
    /// AES encrypt
    /// </summary>
    /// <param name="data">Raw data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <param name="vector">IV,requires 16 bits</param>
    /// <returns>Encrypted string</returns>
    public string? AESEncrypt(string data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));

        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 16, nameof(vector));

        byte[] plainBytes = Encoding.UTF8.GetBytes(data);

        var encryptBytes = AESEncrypt(plainBytes, key, vector);
        if (encryptBytes == null)
        {
            return  null;
        }
        return Convert.ToBase64String(encryptBytes);
    }

    /// <summary>
    /// AES encrypt
    /// </summary>
    /// <param name="data">Raw data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <param name="vector">IV,requires 16 bits</param>
    /// <returns>Encrypted byte array</returns>
    public byte[]? AESEncrypt(byte[] data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));

        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 16, nameof(vector));

        byte[] plainBytes = data;
        byte[] bKey = new byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
        byte[] bVector = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

        byte[]? encryptData = null; // encrypted data
        using Aes Aes = Aes.Create();
        try
        {
            using MemoryStream memory = new MemoryStream();
            using CryptoStream Encryptor = new CryptoStream(memory,
             Aes.CreateEncryptor(bKey, bVector),
             CryptoStreamMode.Write);
            Encryptor.Write(plainBytes, 0, plainBytes.Length);
            Encryptor.FlushFinalBlock();

            encryptData = memory.ToArray();
        }
        catch
        {
            encryptData = null;
        }
        return encryptData;
    }

    /// <summary>
    /// AES encrypt ( no IV)
    /// </summary>
    /// <param name="data">Raw data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <returns>Encrypted string</returns>
    public string AESEncrypt(string data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        using MemoryStream memory = new MemoryStream();
        using Aes aes = Aes.Create();
        byte[] plainBytes = Encoding.UTF8.GetBytes(data);
        byte[] bKey = new byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        aes.Key = bKey;

        using CryptoStream cryptoStream = new CryptoStream(memory, aes.CreateEncryptor(), CryptoStreamMode.Write);
        try
        {
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memory.ToArray());
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///  AES decrypt
    /// </summary>
    /// <param name="data">Encrypted data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <param name="vector">IV,requires 16 bits</param>
    /// <returns>Decrypted string</returns>
    public string? AESDecrypt(string data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));

        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 16, nameof(vector));

        byte[] encryptedBytes = Convert.FromBase64String(data);

        byte[]? decryptBytes = AESDecrypt(encryptedBytes, key, vector);

        if (decryptBytes == null)
        {
            return null;
        }
        return Encoding.UTF8.GetString(decryptBytes);
    }

    /// <summary>
    ///  AES decrypt
    /// </summary>
    /// <param name="data">Encrypted data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <param name="vector">IV,requires 16 bits</param>
    /// <returns>Decrypted byte array</returns>

    public byte[]? AESDecrypt(byte[] data, string key, string vector)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));

        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        Checker.Argument.IsNotEmpty(vector, nameof(vector));
        Checker.Argument.IsEqualLength(vector.Length, 16, nameof(vector));

        byte[] encryptedBytes = data;
        byte[] bKey = new byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
        byte[] bVector = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

        byte[]? decryptedData = null; // decrypted data

        using Aes Aes = Aes.Create();
        try
        {
            using MemoryStream memory = new MemoryStream(encryptedBytes);
            using CryptoStream decryptor = new CryptoStream(memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read);
            using MemoryStream tempMemory = new MemoryStream();
            byte[] Buffer = new byte[1024];
            int readBytes = 0;
            while ((readBytes = decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
            {
                tempMemory.Write(Buffer, 0, readBytes);
            }

            decryptedData = tempMemory.ToArray();
        }
        catch
        {
            decryptedData = null;
        }

        return decryptedData;
    }

    /// <summary>
    /// AES decrypt( no IV)
    /// </summary>
    /// <param name="data">Encrypted data</param>
    /// <param name="key">Key, requires 32 bits</param>
    /// <returns>Decrypted string</returns>
    public string AESDecrypt(string data, string key)
    {
        Checker.Argument.IsNotEmpty(data, nameof(data));
        Checker.Argument.IsNotEmpty(key, nameof(key));
        Checker.Argument.IsEqualLength(key.Length, 32, nameof(key));

        byte[] encryptedBytes = Convert.FromBase64String(data);
        byte[] bKey = new byte[32];
        Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

        try
        {
            byte[]? decryptedData = null; // decrypted data

            using MemoryStream memory = new MemoryStream(encryptedBytes);
            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;
            aes.Key = bKey;

            using CryptoStream decryptor = new CryptoStream(memory, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream tempMemory = new MemoryStream();
            byte[] buffer = new byte[1024];
            int readBytes = 0;
            while ((readBytes = decryptor.Read(buffer, 0, buffer.Length)) > 0)
            {
                tempMemory.Write(buffer, 0, readBytes);
            }

            decryptedData = tempMemory.ToArray();
            return Encoding.UTF8.GetString(decryptedData);
        }
        catch
        {
            throw;
        }
    }
}
