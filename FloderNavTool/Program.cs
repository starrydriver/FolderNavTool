using Avalonia;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FloderNavTool
{
    internal sealed class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole(); // 显示控制台
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            AllocConsole(); // 显示控制台
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
