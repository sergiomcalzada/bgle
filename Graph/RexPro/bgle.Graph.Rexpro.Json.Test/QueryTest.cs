using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Json.Test
{
    [TestClass]
    public class QueryTest : BaseQueryTest
    {
        public QueryTest()
            : base(new RexProJsonSerializer())
        {
        }

        [TestMethod]
        public override void CanQueryTitanGrahpClassTest()
        {
            base.CanQueryTitanGrahpClassTest();
        }
    }
}