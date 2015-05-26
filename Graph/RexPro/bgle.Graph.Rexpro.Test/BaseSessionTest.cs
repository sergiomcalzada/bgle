using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseSessionTest: BaseTest
    {

        public BaseSessionTest(IRexProSerializer serializer)
            : base(serializer)
        {

        }
        
        public virtual void CanBeginSession()
        {
            var session = Client.BeginSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.Id);
            Assert.AreNotEqual(session.Id, Guid.Empty);
        }

        public virtual void CanEndSession()
        {
            var session = Client.BeginSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.Id);
            Assert.AreNotEqual(session.Id, Guid.Empty);

            Client.EndSession(session);
            Assert.IsTrue(session.Killed);
        }

        public virtual void CanUsingSession()
        {
            using (var session = Client.BeginSession())
            {
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.Id);
                Assert.AreNotEqual(session.Id, Guid.Empty);
            }
        }
    }
}