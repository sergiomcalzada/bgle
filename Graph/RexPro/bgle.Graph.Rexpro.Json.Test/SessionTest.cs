using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Json.Test
{
    [TestClass]
    public class SessionTest : BaseSessionTest
    {
        public SessionTest()
            : base(new RexProJsonSerializer())
        {
        }

        [TestMethod]
        public override void CanBeginSession()
        {
            base.CanBeginSession();
        }

        [TestMethod]
        public override void CanEndSession()
        {
            base.CanEndSession();
        }

        [TestMethod]
        public override void CanUsingSession()
        {
            base.CanUsingSession();
        }


    }
}