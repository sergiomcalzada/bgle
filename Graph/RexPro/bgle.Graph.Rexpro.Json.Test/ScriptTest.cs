﻿using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Json.Test
{
    [TestClass]
    public class ScriptTest : BaseScriptTest
    {
        

        public ScriptTest()
            : base(new RexProJsonSerializer())
        {
        }

        [TestMethod]
        public override void CanRunQuery()
        {
            base.CanRunQuery();
        }

        [TestMethod]
        public override void CanRunWithDictionaryBindingsQuery()
        {
            base.CanRunWithDictionaryBindingsQuery();
        }

        [TestMethod]
        public override void CanRunQueryInSession()
        {
            base.CanRunQueryInSession();
        }

        [TestMethod]
        public override void CanRunQueryWithDictionaryBindingsInSession()
        {
            base.CanRunQueryWithDictionaryBindingsInSession();
        }

        
    }
}