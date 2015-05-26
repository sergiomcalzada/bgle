using System;
using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bgle.Graph.Rexpro.Test
{
    public class BaseTest
    {
        protected RexProClient Client;


        public BaseTest(IRexProSerializer serializer)
        {
            this.Client = new RexProClient(serializer);
        }

        
    }

    public class BaseAssemblyInitializer
    {
        private static Process rexterProcess;

        private const string RexterServerPath = @"Graph\RexPro\rexster-server-2.6.0";
        private const string RexterServerBatName =  @"Start.bat";
        protected static void DoAssemblyInitialize(TestContext context)
        {
            var solutionDir = Path.GetDirectoryName(Path.GetDirectoryName(context.TestDir));
            var serverPath = Path.Combine(solutionDir, RexterServerPath);
            var batPath = Path.Combine(serverPath, RexterServerBatName);
            var processInfo = new ProcessStartInfo(batPath)
                                  {
                                      CreateNoWindow = false,
                                      UseShellExecute = true,
                                      RedirectStandardError = false,
                                      RedirectStandardOutput = false,
                                      WorkingDirectory = serverPath
                                  }
                                  ;
            rexterProcess = Process.Start(processInfo);
           
            
            
        }

        protected static void DoAssemblyCleanup()
        {
            rexterProcess.CloseMainWindow();
            rexterProcess.Close();
        }
    }
}