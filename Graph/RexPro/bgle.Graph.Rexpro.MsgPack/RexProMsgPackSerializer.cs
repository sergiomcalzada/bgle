using System;
using System.IO;
using System.Linq;
using System.Text;

using bgle.Graph.Rexpro.protocol.msg;

using MsgPack;
using MsgPack.Serialization;

namespace bgle.Graph.Rexpro.MsgPack
{
    public class RexProMsgPackSerializer : IRexProSerializer
    {
        private readonly MessagePackSerializer<object[]> serializer;

        public RexProMsgPackSerializer()
        {
            this.SerializerType = SerializerType.MsgPack;
            this.serializer = MessagePackSerializer.Get<object[]>();
        }

        public SerializerType SerializerType { get; private set; }

        public byte[] Serialize<T>(T requestMessage) where T : RexProMessage
        {
            using (var stream = new MemoryStream())
            {
                serializer.Pack(stream, requestMessage.GetArray());
                stream.Position = 0;
                return stream.ToArray();
            }

        }

        public T DeSerialize<T>(byte[] headerBytes, byte[] responseBytes) where T : RexProMessage
        {

            var parsed = this.Parse(responseBytes);
            var result = Activator.CreateInstance<T>();
            if (typeof(T) == typeof(SessionResponseMessage))
            {
                //var languages = (System.Collections.IEnumerable)parsed[3];
                //json[3] = (from object language in languages select language.ToString()).ToArray();
            }
            else if (typeof(T) == typeof(ScriptResponseMessage))
            {
                var scriptResult = parsed[3];
                parsed[3] = new RexProScriptResult(scriptResult);

                var scriptBindings = parsed[4];
                parsed[4] = new RexProBindings(); //TODO: transforms bindings from response
            }
            result.Build(parsed);
            return result;
        }

        public ErrorResponseMessage Error(byte[] headerBytes, byte[] responseBytes)
        {
            var parsed = this.Parse(responseBytes);
            parsed[2] = (ErrorResponseMessageFlag)((MessagePackObject)parsed[2]).AsDictionary()["flag"].AsByte();
            
            var result = new ErrorResponseMessage();
            result.Build(parsed);
            return result;

        }

        private object[] Parse(byte[] responseBytes)
        {
            using (var stream = new MemoryStream(responseBytes))
            {
                var result = serializer.Unpack(stream);
                result[0] = new Guid(((MessagePackObject)result[0]).AsBinary());
                result[1] = new Guid(((MessagePackObject)result[1]).AsBinary());

                return result;
            }
        }
    }
}