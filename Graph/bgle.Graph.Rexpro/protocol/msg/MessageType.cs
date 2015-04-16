namespace bgle.Graph.Rexpro.Core.protocol.msg
{
    /// <summary>
    /// Values that represent standard message types for RexPro.
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        ///     Represents an Error message.
        /// </summary>
        Error = 0,

        /// <summary>
        ///     Represents a request to open or close a session.
        /// </summary>
        SessionRequest = 1,

        /// <summary>
        ///     Represents a response to a session request with a newly defined session and available ScriptEngine
        ///     languages or a closed session confirmation..
        /// </summary>
        SessionResponse = 2,

        /// <summary>
        ///     Represents a request to process a script.
        /// </summary>
        ScriptRequest = 3,

        /// <summary>
        ///     Represents a response to a script request that formats results to MsgPack format.
        /// </summary>
        ScriptResponse = 5,
    }
}