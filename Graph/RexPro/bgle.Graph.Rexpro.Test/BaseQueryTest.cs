using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseQueryTest : BaseTest
    {
        public BaseQueryTest(IRexProSerializer serializer)
            : base(serializer)
        {
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