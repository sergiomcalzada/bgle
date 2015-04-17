using System;
using System.IO;
using System.Text;

using bgle.Graph.Rexpro.protocol.msg;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace bgle.Graph.Rexpro.Json
{
    public class RexProJsonSerializer : IRexProSerializer
    {
        private readonly JsonSerializer serializer;

        public RexProJsonSerializer()
        {
            this.SerializerType = SerializerType.Json;
            this.serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public SerializerType SerializerType { get; private set; }

        public byte[] Serialize<T>(T requestMessage) where T : RexProMessage
        {
            
            using (var writer = new StringWriter())
            {
                this.serializer.Serialize(writer, requestMessage.GetArray());
                var json = writer.GetStringBuilder().ToString();
                var body = Encoding.UTF8.GetBytes(json);
                return body;
            }

        }

        public T DeSerialize<T>(byte[] headerBytes, byte[] responseBytes) where T : RexProMessage
        {

            var json =this.Parse(responseBytes);
            var result = Activator.CreateInstance<T>();
            result.Build(json);
            return result;
        }

        public ErrorResponseMessage Error(byte[] headerBytes, byte[] responseBytes)
        {
            var json = this.Parse(responseBytes);
            var result = new ErrorResponseMessage((ErrorResponseMessageFlag)(((dynamic)json[2]).flag));
            result.Build(json);
            return result;

        }

        private object[] Parse(byte[] responseBytes)
        {
            var body = Encoding.UTF8.GetString(responseBytes);

            using (var reader = new StringReader(body))
            {
                using (var jReader = new JsonTextReader(reader))
                {
                   return this.serializer.Deserialize<object[]>(jReader);
                }
            }
        }
    }
}