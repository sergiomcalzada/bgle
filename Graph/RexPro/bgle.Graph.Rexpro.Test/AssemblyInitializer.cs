using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace bgle.Graph.Rexpro.Test
{
    public class AssemblyInitializer
    {
        private const string RexterServerPath = @"Graph\RexPro\rexster-server-2.6.0";

        private const string RexterServerBatName = @"Start.bat";

        public static AssemblyInitializer Current = new AssemblyInitializer();

        private Process rexterProcess;

        private AssemblyInitializer()
        {
            this.DoAssemblyInitialize();
        }

        ~AssemblyInitializer()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            // Run at end
            this.DoAssemblyCleanup();
        }

        private void DoAssemblyInitialize()
        {
            var solutionDir = this.GetSolutionDir();
            var serverPath = solutionDir + RexterServerPath;
            var batPath = Path.Combine(serverPath, RexterServerBatName);
            var processInfo = new ProcessStartInfo(batPath)
                                  {
                                      CreateNoWindow = true,
                                      UseShellExecute = true,
                                      RedirectStandardError = false,
                                      RedirectStandardOutput = false,
                                      WorkingDirectory = serverPath,
                                      WindowStyle = ProcessWindowStyle.Hidden
                                  };
            this.rexterProcess = Process.Start(processInfo);
        }

        private string GetSolutionDir()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream("bgle.Graph.Rexpro.Test.solutiondir.txt");
            if (resourceStream == null) throw new FileNotFoundException("resourceStream");
            using (var reader = new StreamReader(resourceStream))
            {
                return reader.ReadLine().Trim();
            }
        }

        private void DoAssemblyCleanup()
        {
            this.rexterProcess.CloseMainWindow();
            this.rexterProcess.Close();
        }
    }
}