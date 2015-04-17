using System;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class SessionRequestMessage : RexProRequestMessage<SessionRequestMessageMeta>
    {
        [RexProSerialize]
        public String Username { get; set; }

        [RexProSerialize]
        public String Password { get; set; }

        public SessionRequestMessage()
            : base(new SessionRequestMessageMeta(), MessageType.SessionRequest)
        {
        }

        public override object[] GetArray()
        {
            var result = base.GetArray();
            Array.Resize(ref result, result.Length + 2);
            result[result.Length - 2] = Username;
            result[result.Length - 1] = Password;
            return result;
        }
    }

    public class SessionRequestMessageMeta : IRexProRequestMessageMeta
    {
        [RexProSerialize]
        public string GraphName { get; set; }

        [RexProSerialize]
        public string GraphObjName { get; set; }

        [RexProSerialize]
        public bool KillSession { get; set; }

        public SessionRequestMessageMeta()
        {
            this.KillSession = false;
        }

        
    }
}