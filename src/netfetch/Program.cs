using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using netfetch.util;
using netfetch.Windows;


var cmd = new RootCommand("netfetch");
cmd.Handler = CommandHandler.Create(HandleFetch);

cmd.InvokeAsync(args);

static void HandleFetch(string[] args)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        LinuxHandleFetch(args);
        return;
    }
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        WindowsHandleFetch(args);
    }
}

[SupportedOSPlatform("Windows")]
static void WindowsHandleFetch(string[] args)
{
    WinSystemInfo sysInfo = new WinSystemInfo();
    sysInfo.Fetch(args);
}

static void LinuxHandleFetch(string[] args)
{
    // TODO:

}

