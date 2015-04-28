using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    [TestClass]
    public class QueryTest : BaseQueryTest
    {
        public QueryTest()
            : base(new RexProMsgPackSerializer())
        {
        }

        [TestMethod]
        public override void CanQueryTitanGrahpClassTest()
        {
            base.CanQueryTitanGrahpClassTest();
        }
    }
}