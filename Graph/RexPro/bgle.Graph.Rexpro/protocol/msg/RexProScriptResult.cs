namespace bgle.Graph.Rexpro.protocol.msg
{
    public class RexProScriptResult
    {
        public object Value { get; private set; }

        public RexProScriptResult(object value)
        {
            this.Value = value;
        }
    }
}