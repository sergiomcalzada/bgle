namespace bgle.Contracts.Encoder
{
    public interface IEncoder
    {
        string Encode(byte[] input);

        byte[] Decode(string data);
    }
}