using System.Collections.Generic;

namespace bgle.Graph.Rexpro.Core.protocol.msg
{
    public class RexProMessageMeta : Dictionary<string, object>
    {
        public T Get<T>(string key)
        {
            return (T)this.Get(key);
        }
    }
}