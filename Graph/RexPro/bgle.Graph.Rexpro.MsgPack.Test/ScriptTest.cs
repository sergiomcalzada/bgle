using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    public class ScriptTest : BaseScriptTest
    {


        public ScriptTest()
            : base(new RexProMsgPackSerializer())
        {
        }

        [Fact]
        public override void CanRunQuery()
        {
            base.CanRunQuery();
        }

        [Fact]
        public override void CanRunWithDictionaryBindingsQuery()
        {
            base.CanRunWithDictionaryBindingsQuery();
        }

        [Fact]
        public override void CanRunQueryInSession()
        {
            base.CanRunQueryInSession();
        }

        [Fact]
        public override void CanRunQueryWithDictionaryBindingsInSession()
        {
            base.CanRunQueryWithDictionaryBindingsInSession();
        }

        
    }
}