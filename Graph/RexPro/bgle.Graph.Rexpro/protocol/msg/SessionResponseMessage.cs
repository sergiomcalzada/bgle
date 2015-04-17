using System;
using System.Collections.Generic;
using System.Linq;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class SessionResponseMessage : RexProResponseMessage<SessionResponseMessageMeta>
    {
        public String[] Languages;

        public SessionResponseMessage()
            : base(new SessionResponseMessageMeta(), MessageType.SessionResponse)
        {
            this.Languages = new string[0];
        }

        public override void Build(object[] response)
        {
            base.Build(response);
            var languages = (System.Collections.IEnumerable)response[3];
            this.Languages = (from object language in languages select language.ToString()).ToArray();
        }
    }

    public class SessionResponseMessageMeta : IRexProResponseMessageMeta
    {
    }
}