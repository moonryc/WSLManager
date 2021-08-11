using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;

namespace WSLManager.WPF
{
    public class Setup: MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return null;
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            return null;
        }
    }
}