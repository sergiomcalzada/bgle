using System;
using System.IO;
using System.Linq;
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

            var json = this.Parse(responseBytes);
            var result = Activator.CreateInstance<T>();
            if (typeof(T) == typeof(SessionResponseMessage))
            {
                var languages = (System.Collections.IEnumerable)json[3];
                json[3] = (from object language in languages select language.ToString()).ToArray();
            }
            else if (typeof(T) == typeof(ScriptResponseMessage))
            {
                var scriptResult = json[3];
                json[3] = new RexProScriptResult(scriptResult);

                var scriptBindings = json[4];
                json[4] = new RexProBindings(); //TODO: transforms bindings from response
            }
            result.Build(json);
            return result;
        }

        public ErrorResponseMessage Error(byte[] headerBytes, byte[] responseBytes)
        {
            var json = this.Parse(responseBytes);
            json[2] = (ErrorResponseMessageFlag)(((dynamic)json[2]).flag);
            var result = new ErrorResponseMessage();
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