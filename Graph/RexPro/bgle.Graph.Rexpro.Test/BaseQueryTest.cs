using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseQueryTest
    {
        protected readonly RexProClient Client;

        public BaseQueryTest(IRexProSerializer serializer)
        {
            this.Client = new RexProClient(serializer);
        }

        public virtual void CanQueryTitanTest()
        {
            var result = this.Client.Query("q.GetClass()");
            Assert.IsNotNull(result);
            Assert.IsTrue(long.Parse(result.Value.ToString()).Equals(3));
        }
    }
}