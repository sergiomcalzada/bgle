using System.Diagnostics;
using System.IO;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseTest
    {
        protected RexProClient Client;

        protected AssemblyInitializer Initializer;

        public BaseTest(IRexProSerializer serializer)
        {
            this.Client = new RexProClient(serializer);
            this.Initializer = AssemblyInitializer.Current;
        }

        
    }

    
}