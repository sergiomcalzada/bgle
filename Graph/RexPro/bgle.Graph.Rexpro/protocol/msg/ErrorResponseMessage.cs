namespace bgle.Graph.Rexpro.protocol.msg
{
    public enum ErrorResponseMessageFlag
    {
        INVALID_MESSAGE_ERROR = 0,
        INVALID_SESSION_ERROR = 1,
        SCRIPT_FAILURE_ERROR = 2,
        AUTH_FAILURE_ERROR = 3,
        GRAPH_CONFIG_ERROR = 4,
        CHANNEL_CONFIG_ERROR = 5,
        RESULT_SERIALIZATION_ERROR = 6,
        UNKNOWN_ERROR = 7,
    }

    public class ErrorResponseMessage : RexProMessage<ErrorResponseMessageMeta>
    {
        public string ErrorMessage { get; private set; }

        public ErrorResponseMessage(ErrorResponseMessageFlag flag)
            : base(new ErrorResponseMessageMeta(flag), MessageType.Error)
        {
        }

        public override void Build(object[] response)
        {
            base.Build(response);
            this.ErrorMessage = response[3].ToString();
        }
    }

    public class ErrorResponseMessageMeta : IRexProMessageMeta
    {
        public ErrorResponseMessageFlag Flag { get; private set; }

        public ErrorResponseMessageMeta(ErrorResponseMessageFlag flag)
        {
            this.Flag = flag;
        }
    }
}