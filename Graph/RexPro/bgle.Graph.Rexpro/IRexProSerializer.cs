using bgle.Graph.Rexpro.protocol.msg;

namespace bgle.Graph.Rexpro
{
    public interface IRexProSerializer
    {
        SerializerType SerializerType { get; }

        byte[] Serialize<T>(T requestMessage) where T : RexProMessage;

        T DeSerialize<T>(byte[] headerBytes, byte[] responseBytes) where T : RexProMessage;

        ErrorResponseMessage Error(byte[] headerBytes, byte[] responseBytes);
    }
}