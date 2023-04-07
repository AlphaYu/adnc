namespace System;

public static class StreamExtensions
{
    /// <summary>
    /// Stream write all bytes 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="byts"></param>
    public static void WriteAll(this Stream stream, byte[] byts)
    {
        stream.Write(byts, 0, byts.Length);
    }
}
