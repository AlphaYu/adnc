using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// Create decryptionKey
    /// </summary>
    /// <param name="length">decryption key length range is 16 -48</param>
    /// <returns>DecryptionKey</returns>
    public string CreateDecryptionKey(int length)
    {
        Checker.Argument.IsNotOutOfRange(length, 16, 48, nameof(length));
        return CreateMachineKey(length);
    }

    /// <summary>
    /// Create validationKey
    /// </summary>
    /// <param name="length"></param>
    /// <returns>ValidationKey</returns>
    public string CreateValidationKey(int length)
    {
        Checker.Argument.IsNotOutOfRange(length, 48, 128, nameof(length));
        return CreateMachineKey(length);
    }

    /// <summary>
    /// <para>Use cryptographic service providers to implement encryption to generate random numbers</para>
    /// <para>
    /// Description:
    /// validationKey The value can be 48 to 128 characters long.It is strongly recommended to use the longest key available
    /// decryptionKey The value can be 16 to 48 characters long.It is recommended to use 48 characters long
    /// </para>
    /// <para>
    /// How to use:
    /// string decryptionKey = EncryptManager.CreateMachineKey(48);
    /// string validationKey = EncryptManager.CreateMachineKey(128);
    /// </para>
    /// </summary>
    /// <param name="length">Length</param>
    /// <returns></returns>
    private string CreateMachineKey(int length)
    {

        byte[] random = new byte[length / 2];

        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);

        StringBuilder machineKey = new StringBuilder(length);
        for (int i = 0; i < random.Length; i++)
        {
            machineKey.Append(string.Format("{0:X2}", random[i]));
        }
        return machineKey.ToString();
    }
}
