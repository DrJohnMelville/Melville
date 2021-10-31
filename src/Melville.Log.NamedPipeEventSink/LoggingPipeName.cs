using System;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Melville.Log.NamedPipeEventSink;

public class LoggingPipeName
{
    public string ServerName =>
        $"\\\\.\\pipe\\MelvilleSoftware\\Logging\\{WindowsIdentity.GetCurrent().User?.Value}";

    public string ClientPipeName(string suffix) => $"{ServerName}-{suffix}";

    public string NewClientPipeName(int length) => 
        ClientPipeName(RandomBytesToTextString(GetRandomBytes(length)));

    private const string NameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private static string RandomBytesToTextString(byte[] randomBytes)
    {
        var ret = new char[randomBytes.Length];
        for (int i = 0; i < randomBytes.Length; i++)
        {
            ret[i] = NameChars[randomBytes[i] % NameChars.Length];
        }
        return new string(ret);
    }

    private readonly Lazy<RNGCryptoServiceProvider> crypto = 
        new Lazy<RNGCryptoServiceProvider>(()=>new RNGCryptoServiceProvider());
    private byte[] GetRandomBytes(int length)
    {
        var randomBytes = new byte[length];
        crypto.Value.GetBytes(randomBytes, 0, length);
        return randomBytes;
    }
}