using System;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class ScriptResponseMessage : RexProResponseMessage<ScriptResponseMessageMeta>
    {
        

        public RexProScriptResult Result { get; set; }
        public RexProBindings Bindings { get; set; }

        public ScriptResponseMessage()
            : base(new ScriptResponseMessageMeta(), MessageType.ScriptResponse)
        {
        }

        public override void Build(object[] response)
        {
            base.Build(response);
            this.Result = (RexProScriptResult)response[3];
            this.Bindings = (RexProBindings)response[4];
        }

        
    }

    public class ScriptResponseMessageMeta : IRexProResponseMessageMeta
    {
    }
}