using System;
using System.Collections.Generic;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class SessionRequestMessage : RexProRequestMessage<SessionRequestMessageMeta>
    {
        public String Username { get; set; }

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
        public string GraphName { get; set; }

        public string GraphObjName { get; set; }

        public bool KillSession { get; set; }

        public SessionRequestMessageMeta()
        {
            this.KillSession = false;
        }

        public IDictionary<string, object> AsDictionary()
        {
            var dic = new Dictionary<string, object>();

            dic.AddPascalCase(() => this.GraphName);
            dic.AddPascalCase(() => this.GraphObjName);
            dic.AddPascalCase(() => this.KillSession);
            return dic;
        }
    }
}