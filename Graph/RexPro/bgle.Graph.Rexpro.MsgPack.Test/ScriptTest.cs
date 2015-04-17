using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.MsgPack.Test
{
    [TestClass]
    public class ScriptTest
    {
        [TestMethod]
        public void CanRunQuery()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            var result = client.Query("number = 1+2");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.Equals((long)3));
        }

        [TestMethod]
        public void CanRunWithBindingsQuery()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            var result = client.Query("three = one + two", new Dictionary<string, object>
                                                                {
                                                                    {"one", 1},
                                                                    {"two", 2},
                                                                });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value.Equals((long)3));
        }

        [TestMethod]
        public void CanRunQueryInSession()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            using (var session = client.BeginSession())
            {
                var result = client.Query("number = 1+2", null, session);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Value.Equals((long)3)); 
            }
            
        }

        [TestMethod]
        public void CanRunQueryWithBindingsInSession()
        {
            var client = new RexProClient(new RexProMsgPackSerializer());
            using (var session = client.BeginSession())
            {
                var result = client.Query("three = one + two", new Dictionary<string, object>
                                                                {
                                                                    {"one", 1},
                                                                    {"two", 2},
                                                                }, session);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Value.Equals((long)3));
            }

        }
    }
}