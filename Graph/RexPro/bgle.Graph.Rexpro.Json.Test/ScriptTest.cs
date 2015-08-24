using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.Json.Test
{
    public class ScriptTest : BaseScriptTest
    {
        

        public ScriptTest()
            : base(new RexProJsonSerializer())
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