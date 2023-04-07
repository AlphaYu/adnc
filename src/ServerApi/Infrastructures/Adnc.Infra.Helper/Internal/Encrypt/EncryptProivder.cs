namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    internal EncryptProivder()
    {
    }

    /// <summary>
    /// The single Random Generator
    /// </summary>
    private Random? random;

    /// <summary>
    /// Generate a random key
    /// </summary>
    /// <param name="n">key length，IV is 16，Key is 32</param>
    /// <returns>return random value</returns>
    private string GetRandomStr(int length)
    {
        char[] arrChar = new char[]{
   'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
   '0','1','2','3','4','5','6','7','8','9',
   'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
  };

        StringBuilder num = new();

        //New stronger Random Generator
        if (random == null)
        {
            random = new Random();
        }

        for (int i = 0; i < length; i++)
        {
            num.Append(arrChar[random.Next(0, arrChar.Length)].ToString());
        }

        return num.ToString();
    }
}
