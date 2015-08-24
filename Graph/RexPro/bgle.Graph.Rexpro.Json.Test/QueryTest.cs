using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.Json.Test
{
    public class QueryTest : BaseQueryTest
    {
        public QueryTest()
            : base(new RexProJsonSerializer())
        {
        }

        [Fact]
        public override void CanQueryTitanGrahpClassTest()
        {
            base.CanQueryTitanGrahpClassTest();
        }
    }
}