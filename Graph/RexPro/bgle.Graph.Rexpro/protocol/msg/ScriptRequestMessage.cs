using System;
using System.Collections.Generic;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class ScriptRequestMessage : RexProRequestMessage<ScriptRequestMessageMeta>
    {
        [RexProSerialize]
        public string LanguageName { get; set; }

        [RexProSerialize]
        public string Script { get; private set; }

        [RexProSerialize]
        public RexProBindings Bindings { get; private set; }

        public ScriptRequestMessage(string script, IDictionary<string, object> bindings)
            : base(new ScriptRequestMessageMeta(), MessageType.ScriptRequest)
        {
            this.Bindings = new RexProBindings(bindings);
            this.Script = script;
        }

        public override object[] GetArray()
        {
            var result = base.GetArray();
            Array.Resize(ref result, result.Length+3);
            result[result.Length - 3] = LanguageName;
            result[result.Length - 2] = Script;
            result[result.Length - 1] = Bindings;
            return result;
        }
    }

    public class ScriptRequestMessageMeta : IRexProRequestMessageMeta
    {
        [RexProSerialize]
        public bool InSession { get; set; }

        [RexProSerialize]
        public bool Isolate { get; set; }

        [RexProSerialize]
        public bool Transaction { get; set; }

        [RexProSerialize]
        public string GraphName { get; set; }

        [RexProSerialize]
        public string GraphObjName { get; set; }

        [RexProSerialize]
        public string Console { get; set; }

        public ScriptRequestMessageMeta()
        {
            this.GraphName = "graph";
            this.GraphObjName = "g";
        }

        
    }
}