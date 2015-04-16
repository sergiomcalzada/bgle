using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using bgle.Graph.Rexpro.Core.protocol.msg;

namespace bgle.Graph.Rexpro.Core
{
    public class RexProClient
    {
        private static readonly IDictionary<MessageType, MessageType> ExpectedResponseMessageType = new Dictionary<MessageType, MessageType>
        {
            { MessageType.SessionRequest, MessageType.SessionResponse },
            { MessageType.ScriptRequest, MessageType.ScriptResponse }
        };

        private readonly string host;

        private readonly SerializerType serializerType;

        private readonly int port;

        private const byte ProtocolVersion = 1;
        private const SerializerType DefaultSerializerType = SerializerType.Json;
        private const int DefaultPort = 8184;

        public RexProClient(string host)
            : this(host, DefaultPort, DefaultSerializerType)
        {
        }

        public RexProClient(string host, int port, SerializerType serializerType)
        {
            this.host = host;
            this.serializerType = serializerType;
            this.port = port;
        }

        private readonly ConcurrentDictionary<Guid, TcpClient> connections = new ConcurrentDictionary<Guid, TcpClient>();


        public dynamic Query(string script, Dictionary<string, object> bindings = null, RexProSession session = null, bool transaction = true)
        {
            var request = new ScriptRequestMessage(script, bindings);
            return this.ExecuteScript(request, session, transaction).Result;
        }

        public ScriptResponseMessage ExecuteScript(ScriptRequestMessage script, RexProSession session = null, bool transaction = true)
        {
            script.MetaSetInSession(session != null);
            script.MetaSetIsolate(session == null);
            script.MetaSetTransaction(transaction);


            if (session == null)
            {
                if (script.MetaGetGraphName() == null) script.MetaSetGraphName("graph");
                if (script.MetaGetGraphObjName() == null) script.MetaSetGraphObjName("g");
            }
            else
            {
                script.Session = session.Id;
            }

            return this.SendRequest<ScriptRequestMessage, ScriptResponseMessage>(script);
        }

        private TResponse SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : RexProMessage
            where TResponse : RexProMessage
        {

            //var messageBytes = BuildRequestMessageBuffer(message, out requestMessageType, out messageLength);

            var connection = request.Session != Guid.Empty
                                ? connections.GetOrAdd(request.Session, _ => new TcpClient())
                                : new TcpClient(this.host, this.port);

            try
            {
                if (!connection.Connected) connection.Connect(this.host, this.port);
                connection.Client.Send(request.ToByteArray());
                return ParseResponse<TResponse>(connection, request.MessageType);
            }
            finally
            {
                if (request.Session == Guid.Empty)
                {
                    connection.Close();
                }
            }
        }

        private T ParseResponse<T>(TcpClient connection, MessageType requestMessageType)
            where T : RexProMessage
        {
            T result;


            var headerBytes = new byte[RexProMessage.MESSAGE_HEADER_SIZE];
            var bytesRead = 0;
            var networkStream = connection.GetStream();

            while (bytesRead != RexProMessage.MESSAGE_HEADER_SIZE)
            {

                var bytes = networkStream.Read(headerBytes, bytesRead, RexProMessage.MESSAGE_HEADER_SIZE - bytesRead);
                bytesRead += bytes;
            }

            var expectedResponseMessageType = ExpectedResponseMessageType[requestMessageType];

            var protocol = headerBytes[RexProMessage.MESSAGE_HEADER_PROTOCOL];
            var serializer = (SerializerType)headerBytes[RexProMessage.MESSAGE_HEADER_SERIALIZER];
            var messageType = (MessageType)headerBytes[RexProMessage.MESSAGE_HEADER_MESSAGE_TYPE];

            if ((protocol != ProtocolVersion) || (serializer != this.serializerType))
            {
                throw new RexProException("Unexpected message header.");
            }

            var messageLength = (headerBytes[7] << 24) | (headerBytes[8] << 16) | (headerBytes[9] << 8) | (headerBytes[10]);

            var buffer = new byte[1024];
            bytesRead = 0;

            using (var stream = new MemoryStream())
            {
                while (bytesRead < messageLength)
                {
                    var bytes = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytes > 0)
                    {
                        stream.Write(buffer, 0, bytes);
                        bytesRead += bytes;
                    }
                }

                var readedBytes = stream.ToArray();

                if (messageType == MessageType.Error)
                {
                    var error = new ErrorResponseMessage();
                    //error.LoadJson(json);
                    throw new RexProException(error.ErrorMessage);
                }

                if (messageType != expectedResponseMessageType)
                {
                    var msg = string.Format(CultureInfo.InvariantCulture,
                                            "Unexpected message type '{0}', expected '{1}'.",
                                            messageType, expectedResponseMessageType);
                    throw new RexProException(msg);
                }

                result = Activator.CreateInstance<T>();
                //result.LoadJson(json);
            }

            return result;
        }
    }
}
