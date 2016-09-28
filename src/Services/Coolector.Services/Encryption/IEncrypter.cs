namespace Coolector.Services.Encryption
{
    public interface IEncrypter
    {
        string GetRandomSecureKey();
    }
}