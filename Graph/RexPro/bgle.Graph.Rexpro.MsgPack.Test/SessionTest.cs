using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    [TestClass]
    public class SessionTest : BaseSessionTest
    {
        public SessionTest()
            : base(new RexProMsgPackSerializer())
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