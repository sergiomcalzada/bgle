using System;
using System.Collections.Generic;
using System.Linq;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class SessionResponseMessage : RexProResponseMessage<SessionResponseMessageMeta>
    {
        public string[] Languages;

        public SessionResponseMessage()
            : base(new SessionResponseMessageMeta(), MessageType.SessionResponse)
        {
            this.Languages = new string[0];
        }

        public override void Build(object[] response)
        {
            base.Build(response);
            this.Languages =  (string[])response[3];
        }
    }

    public class SessionResponseMessageMeta : IRexProResponseMessageMeta
    {
    }
}