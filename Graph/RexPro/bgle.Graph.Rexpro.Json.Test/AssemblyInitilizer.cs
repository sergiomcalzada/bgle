using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bgle.Graph.Rexpro.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Json.Test
{
    [TestClass]
    public class AssemblyInitilizer : BaseAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            DoAssemblyInitialize(context);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup(TestContext context)
        {
            DoAssemblyCleanup();
        }
    }
}
