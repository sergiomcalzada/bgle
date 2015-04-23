using System.Collections;
using System.Collections.Generic;

namespace bgle.Graph.Rexpro.protocol.msg
{
    

    public interface IRexProMessageMeta
    {
        
    }

    public interface IRexProRequestMessageMeta : IRexProMessageMeta
    {
        IDictionary<string, object> AsDictionary();
    }

    public interface IRexProResponseMessageMeta : IRexProMessageMeta
    {

    }
}