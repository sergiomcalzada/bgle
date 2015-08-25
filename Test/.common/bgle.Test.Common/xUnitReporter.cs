using System;

using TestStack.BDDfy.Reporters;

using Xunit.Abstractions;

namespace bgle.Test.Common
{
    public class XunitReporter : TextReporter
    {
        private readonly ITestOutputHelper output;

        public override ConsoleColor ForegroundColor
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                Console.ForegroundColor = value;
            }
        }

        public XunitReporter(ITestOutputHelper outputHelper)
        {
            this.output = outputHelper;
        }

        protected override void Write(string text, params object[] args)
        {
            this.output.WriteLine(text, args);
        }

        protected override void WriteLine(string text = null)
        {
            if (text != null)
            {
                this.output.WriteLine(text);
            }
        }

        protected override void WriteLine(string text, params object[] args)
        {
            this.output.WriteLine(text, args);
        }
    }
}