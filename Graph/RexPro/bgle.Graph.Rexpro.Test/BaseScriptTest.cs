﻿using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseScriptTest : BaseTest
    {
        
        public BaseScriptTest(IRexProSerializer serializer)
            : base(serializer)
        {

        }

        public virtual void CanRunQuery()
        {
            var result = this.Client.Query("number = 1+2");
            Assert.IsNotNull(result);
            Assert.IsTrue(long.Parse(result.Value.ToString()).Equals(3));
        }

        public virtual void CanRunWithDictionaryBindingsQuery()
        {
            var result = this.Client.Query("three = one + two", new Dictionary<string, object> { { "one", 1 }, { "two", 2 }, });
            Assert.IsNotNull(result);
            Assert.IsTrue(long.Parse(result.Value.ToString()).Equals(3));
        }

        public virtual void CanRunQueryInSession()
        {
            using (var session = this.Client.BeginSession())
            {
                var result = this.Client.Query("number = 1+2", null, session);
                Assert.IsNotNull(result);
                Assert.IsTrue(long.Parse(result.Value.ToString()).Equals(3));
            }
        }

        public virtual void CanRunQueryWithDictionaryBindingsInSession()
        {
            using (var session = this.Client.BeginSession())
            {
                var result = this.Client.Query("three = one + two", new Dictionary<string, object> { { "one", 1 }, { "two", 2 }, }, session);
                Assert.IsNotNull(result);
                Assert.IsTrue(long.Parse(result.Value.ToString()).Equals(3));
            }
        }

    }
}