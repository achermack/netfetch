using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

var cmd = new RootCommand("netfetch");
cmd.Handler = CommandHandler.Create(HandleFetch);

cmd.Invoke(args);


static void HandleFetch(string[] args)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        LinuxHandleFetch(args);
        return;
    }
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        OSXHandleFetch(args);
        return;
    }
    WindowsHandleFetch(args);
}




static void WindowsHandleFetch(string[] args)
{
    WinFetch winfetch = new WinFetch();
    winfetch.Fetch(args);
}

static void LinuxHandleFetch(string[] args)
{
    // use the windows fetch command

}


static void OSXHandleFetch(string[] args)
{
    // use the windows fetch command
    Console.WriteLine("hello osx!");
}

