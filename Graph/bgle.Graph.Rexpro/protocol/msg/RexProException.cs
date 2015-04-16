using System;

namespace bgle.Graph.Rexpro.Core.protocol.msg
{
    public class RexProException : Exception
    {
        public RexProException(string message)
            : base(message)
        {
            
        }
    }
}