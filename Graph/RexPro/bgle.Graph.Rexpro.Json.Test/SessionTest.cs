using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.Json.Test
{
    public class SessionTest : BaseSessionTest
    {
        public SessionTest()
            : base(new RexProJsonSerializer())
        {
        }

        [Fact]
        public override void CanBeginSession()
        {
            base.CanBeginSession();
        }

        [Fact]
        public override void CanEndSession()
        {
            base.CanEndSession();
        }

        [Fact]
        public override void CanUsingSession()
        {
            base.CanUsingSession();
        }


    }
}