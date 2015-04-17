using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using bgle.Graph.Rexpro.protocol.msg;

namespace bgle.Graph.Rexpro
{
    public class RexProClient
    {
        private static readonly IDictionary<MessageType, MessageType> ExpectedResponseMessageType = new Dictionary<MessageType, MessageType>
        {
            { MessageType.SessionRequest, MessageType.SessionResponse },
            { MessageType.ScriptRequest, MessageType.ScriptResponse }
        };

        private readonly string host;

        private readonly int port;

        private readonly IRexProSerializer serializer;


        private const int DefaultPort = 8184;

        public RexProClient(IRexProSerializer serializer)
            : this("localhost", DefaultPort, serializer)
        {
        }

        public RexProClient(string host, int port, IRexProSerializer serializer)
        {
            this.host = host;
            this.port = port;
            this.serializer = serializer;
        }

        private readonly ConcurrentDictionary<Guid, TcpClient> connections = new ConcurrentDictionary<Guid, TcpClient>();


        public RexProSession BeginSession()
        {
            var request = new SessionRequestMessage();
            var response = this.SendRequest<SessionRequestMessage, SessionResponseMessage>(request);
            var session = new RexProSession(response.Session);

            this.connections.GetOrAdd(session.Id, _ => new TcpClient(this.host, this.port));
            session.KillSession += (sender, args) =>
            {
                if (!session.Killed) this.EndSession(session);
            };

            return session;
        }

        public void EndSession(RexProSession session)
        {
            var request = new SessionRequestMessage { Session = session.Id, Meta = { KillSession = true } };

            var response = this.SendRequest<SessionRequestMessage, SessionResponseMessage>(request);

            while (this.connections.ContainsKey(session.Id))
            {
                TcpClient s;
                if (this.connections.TryRemove(session.Id, out s))
                {
                    s.Close();
                }
                else
                {
                    Thread.SpinWait(10);
                }
            }

            session.Killed = true;
        }

        public dynamic Query(string script, Dictionary<string, object> bindings = null, RexProSession session = null, bool transaction = true)
        {
            var request = new ScriptRequestMessage(script, bindings);
            return this.ExecuteScript(request, session, transaction).Result;
        }

        private ScriptResponseMessage ExecuteScript(ScriptRequestMessage script, RexProSession session = null, bool transaction = true)
        {
            script.Meta.InSession = session != null;
            script.Meta.Isolate = session == null;
            script.Meta.Transaction = transaction;


            if (session != null)
            {
                script.Session = session.Id;
            }


            return this.SendRequest<ScriptRequestMessage, ScriptResponseMessage>(script);
        }

        private TResponse SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : RexProMessage
            where TResponse : RexProMessage
        {

            var connection = request.Session != Guid.Empty
                                ? this.connections.GetOrAdd(request.Session, _ => new TcpClient(this.host, this.port))
                                : new TcpClient(this.host, this.port);

            try
            {
                var bytes = new byte[RexProMessage.MESSAGE_HEADER_SIZE];
                bytes[RexProMessage.MESSAGE_HEADER_PROTOCOL] = RexProMessage.ProtocolVersion;
                bytes[RexProMessage.MESSAGE_HEADER_SERIALIZER] = (byte)this.serializer.SerializerType;
                bytes[RexProMessage.MESSAGE_HEADER_MESSAGE_TYPE] = (byte)request.MessageType;

                var body = this.serializer.Serialize(request);
                //set the length of the body
                bytes[7] = (byte)((body.Length >> 24) & 255);
                bytes[8] = (byte)((body.Length >> 16) & 255);
                bytes[9] = (byte)((body.Length >> 8) & 255);
                bytes[10] = (byte)(body.Length & 255);

                //Resize and copy the body
                Array.Resize(ref bytes, RexProMessage.MESSAGE_HEADER_SIZE + body.Length);
                Array.Copy(body, 0, bytes, RexProMessage.MESSAGE_HEADER_SIZE, body.Length);

                connection.Client.Send(bytes);
                return this.ParseResponse<TResponse>(connection.GetStream(), request.MessageType);
            }
            finally
            {
                if (request.Session == Guid.Empty)
                {
                    connection.Close();
                }
            }
        }

        private T ParseResponse<T>(Stream networkStream, MessageType requestMessageType)
            where T : RexProMessage
        {
            var headerBytes = new byte[RexProMessage.MESSAGE_HEADER_SIZE];
            var bytesRead = 0;

            while (bytesRead != RexProMessage.MESSAGE_HEADER_SIZE)
            {

                var bytes = networkStream.Read(headerBytes, bytesRead, RexProMessage.MESSAGE_HEADER_SIZE - bytesRead);
                bytesRead += bytes;
            }

            var expectedResponseMessageType = ExpectedResponseMessageType[requestMessageType];

            var responseProtocol = headerBytes[RexProMessage.MESSAGE_HEADER_PROTOCOL];
            var responseSerializer = (SerializerType)headerBytes[RexProMessage.MESSAGE_HEADER_SERIALIZER];
            var responseMessageType = (MessageType)headerBytes[RexProMessage.MESSAGE_HEADER_MESSAGE_TYPE];
            var messageLength = (headerBytes[7] << 24) | (headerBytes[8] << 16) | (headerBytes[9] << 8) | (headerBytes[10]);

            if (responseProtocol != RexProMessage.ProtocolVersion)
            {
                throw new RexProException("Unsupported protol version.");
            }

            if (responseSerializer != this.serializer.SerializerType)
            {
                throw new RexProException("Invalid serializer.");
            }


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

                var responseBytes = stream.ToArray();

                if (responseMessageType != MessageType.Error)
                {
                    return this.serializer.DeSerialize<T>(headerBytes, responseBytes);
                }
                else
                {
                    var error = this.serializer.Error(headerBytes, responseBytes);
                    throw new RexProErrorException("Error message", error);
                }

            }
        }
    }
}
