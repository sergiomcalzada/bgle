using System;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class ScriptResponseMessage : RexProResponseMessage<ScriptResponseMessageMeta>
    {
        private static readonly MsgPackResultConverter Converter = new MsgPackResultConverter();

        public RexProScriptResult Result = new RexProScriptResult();
        public RexProBindings Bindings = new RexProBindings();

        public ScriptResponseMessage()
            : base(new ScriptResponseMessageMeta(), MessageType.ScriptResponse)
        {
        }

        public override void Build(object[] response)
        {
            base.Build(response);
        }

        public static byte[] ConvertResultToBytes(object result)
        {
            return Converter.Convert(result);
        }
    }

    public class ScriptResponseMessageMeta : IRexProResponseMessageMeta
    {
    }
}