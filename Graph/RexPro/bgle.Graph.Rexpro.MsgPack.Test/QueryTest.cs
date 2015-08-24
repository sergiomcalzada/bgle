using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    public class QueryTest : BaseQueryTest
    {
        public QueryTest()
            : base(new RexProMsgPackSerializer())
        {
        }

        [Fact]
        public override void CanQueryTitanGrahpClassTest()
        {
            base.CanQueryTitanGrahpClassTest();
        }
    }
}