using System.Collections.Generic;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class RexProBindings : Dictionary<string, object>
    {
        public RexProBindings()
        {
        }

        public RexProBindings(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }
    }
}