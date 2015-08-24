using System;

using Xunit;

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
            var session = this.Client.BeginSession();
            Assert.NotNull(session);
            Assert.NotNull(session.Id);
            Assert.NotEqual(session.Id, Guid.Empty);
        }

        public virtual void CanEndSession()
        {
            var session = this.Client.BeginSession();
            Assert.NotNull(session);
            Assert.NotNull(session.Id);
            Assert.NotEqual(session.Id, Guid.Empty);

            this.Client.EndSession(session);
            Assert.True(session.Killed);
        }

        public virtual void CanUsingSession()
        {
            using (var session = this.Client.BeginSession())
            {
                Assert.NotNull(session);
                Assert.NotNull(session.Id);
                Assert.NotEqual(session.Id, Guid.Empty);
            }
        }
    }
}