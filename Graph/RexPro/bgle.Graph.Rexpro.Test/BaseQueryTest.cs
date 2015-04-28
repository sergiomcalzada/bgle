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

        public virtual void CanQueryTitanGrahpClassTest()
        {
            this.Client.GraphName = "titanexample";
            var result = this.Client.Query("g.getClass()");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.ToString().Equals("class com.thinkaurelius.titan.graphdb.database.StandardTitanGraph"));
        }
        
    }
}