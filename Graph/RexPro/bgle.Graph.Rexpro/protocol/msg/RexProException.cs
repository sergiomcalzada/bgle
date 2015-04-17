using System;

namespace bgle.Graph.Rexpro.protocol.msg
{
    public class RexProException : Exception
    {
        public RexProException(string message)
            : base(message)
        {
            
        }
    }

    public class RexProErrorException : RexProException
    {
        public ErrorResponseMessage Error { get; private set; }

        public RexProErrorException(string message, ErrorResponseMessage error)
            : base(message)
        {
            this.Error = error;
        }
    }
}