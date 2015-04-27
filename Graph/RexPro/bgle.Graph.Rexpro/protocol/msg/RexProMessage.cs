using System;

namespace bgle.Graph.Rexpro.protocol.msg
{
    

    public class RexProMessage
    {
        public const byte ProtocolVersion = 1;

        public const int MESSAGE_HEADER_SIZE = 11;

        public const int MESSAGE_HEADER_PROTOCOL = 0;

        public const int MESSAGE_HEADER_SERIALIZER = 1;

        public const int MESSAGE_HEADER_MESSAGE_TYPE = 6;

        public MessageType MessageType { get; private set; }

        public Guid Session { get; set; }

        public Guid Request { get; set; }

        protected RexProMessage(MessageType messageType)
        {
            this.MessageType = messageType;
        }

        public virtual object[] GetArray()
        {
            return new object[] { this.Session, this.Request };
        }

        public virtual void Build(object[] response)
        {
            this.Session = new Guid(response[0].ToString());
            this.Request = new Guid(response[1].ToString());
        }
    }

    /// <summary>
    ///     A basic RexProMessage containing the basic components of every message that Rexster can process.
    /// </summary>
    public class RexProMessage<T> : RexProMessage
        where T : IRexProMessageMeta
    {
        public T Meta { get; private set; }

        public RexProMessage(T meta, MessageType messageType)
            : base(messageType)
        {
            this.Meta = meta;
        }
    }

    public class RexProRequestMessage<T> : RexProMessage<T>
        where T : IRexProRequestMessageMeta
    {
        public RexProRequestMessage(T meta, MessageType messageType)
            : base(meta, messageType)
        {
        }

        public override object[] GetArray()
        {
            var result = base.GetArray();
            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = this.Meta.AsDictionary();
            return result;
        }
    }

    public class RexProResponseMessage<T> : RexProMessage<T>
        where T : IRexProResponseMessageMeta
    {
        public RexProResponseMessage(T meta, MessageType messageType)
            : base(meta, messageType)
        {
        }
    }
}