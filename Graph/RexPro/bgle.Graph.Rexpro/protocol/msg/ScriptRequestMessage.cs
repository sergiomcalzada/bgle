using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class ScriptRequestMessage : RexProRequestMessage<ScriptRequestMessageMeta>
    {
        public string LanguageName { get; set; }

        public string Script { get; private set; }

        public RexProBindings Bindings { get; private set; }

        public ScriptRequestMessage(string script, IDictionary<string, object> bindings)
            : base(new ScriptRequestMessageMeta(), MessageType.ScriptRequest)
        {
            this.Bindings = bindings != null ? new RexProBindings(bindings) : new RexProBindings();
            this.Script = script;
            this.LanguageName = "groovy";
        }

        public override object[] GetArray()
        {
            var result = base.GetArray();
            Array.Resize(ref result, result.Length + 3);
            result[result.Length - 3] = LanguageName;
            result[result.Length - 2] = Script;
            result[result.Length - 1] = Bindings;
            return result;
        }
    }

    public class ScriptRequestMessageMeta : IRexProRequestMessageMeta
    {
        public bool InSession { get; set; }

        public bool Isolate { get; set; }

        public bool Transaction { get; set; }

        public string GraphName { get; set; }

        public string GraphObjName { get; set; }

        public string Console { get; set; }

        public IDictionary<string, object> AsDictionary()
        {
            var dic = new Dictionary<string, object>();
            dic.Add(() => this.InSession);
            dic.Add(() => this.Isolate);
            dic.Add(() => this.Transaction);
            dic.Add(() => this.GraphName);
            dic.Add(() => this.GraphObjName);
            dic.Add(() => this.Console);
            return dic;
        }


    }
}