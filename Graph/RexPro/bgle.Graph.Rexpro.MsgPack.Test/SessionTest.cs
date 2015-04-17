using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    [TestClass]
    public class SessionTest
    {
        [TestMethod]
        public void CanBeginSession()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            var session = client.BeginSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.Id);
            Assert.AreNotEqual(session.Id, Guid.Empty);
        }

        [TestMethod]
        public void CanEndSession()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            var session = client.BeginSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.Id);
            Assert.AreNotEqual(session.Id, Guid.Empty);

            client.EndSession(session);
            Assert.IsTrue(session.Killed);
        }

        [TestMethod]
        public void CanUsingSession()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            using (var session = client.BeginSession())
            {
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.Id);
                Assert.AreNotEqual(session.Id, Guid.Empty);
            }

        }
    }
}
