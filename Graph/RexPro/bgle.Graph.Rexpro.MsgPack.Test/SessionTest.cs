using bgle.Graph.Rexpro.Test;

using Xunit;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    
    public class SessionTest : BaseSessionTest
    {
        public SessionTest()
            : base(new RexProMsgPackSerializer())
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