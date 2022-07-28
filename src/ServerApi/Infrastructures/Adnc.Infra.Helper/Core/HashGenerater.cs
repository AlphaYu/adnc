namespace Adnc.Infra.Helper;

public interface IHashGenerater
{ 
    public HashConsistentGenerater ConsistentGenerater { get; }
}

internal class HashGenerater : IHashGenerater
{
    public HashConsistentGenerater ConsistentGenerater => HashConsistentGenerater.Instance;

    internal HashGenerater()
    { 
    }
}

public enum HashType
{
    /// <summary>
    /// MD5
    /// </summary>
    MD5 = 0,

    /// <summary>
    /// SHA1
    /// </summary>
    SHA1 = 1,

    /// <summary>
    /// SHA256
    /// </summary>
    SHA256 = 2,

    /// <summary>
    /// SHA384
    /// </summary>
    SHA384 = 3,

    /// <summary>
    /// SHA512
    /// </summary>
    SHA512 = 4
}