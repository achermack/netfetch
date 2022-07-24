using System.Runtime.Versioning;
using netfetch.util;
using netfetch.Windows;
using Xunit;

namespace netfetch.Tests;
[SupportedOSPlatform("windows")]
public class ISystemInfoTests
{
    [Fact]
    public void Test_WinSystemInfo_Fetch()
    {
        WinSystemInfo sysInfo = new WinSystemInfo();
        sysInfo.Fetch(new string[] { });
        Assert.True(true);
    }

    [Fact]
    public void Test_WinSystemInfo_SystemInformation()
    {
        SystemInfo sysInfo = new WinSystemInfo();
        sysInfo.Fetch(new string[] { });
        Assert.NotNull(sysInfo.SystemInformation);
        Assert.NotEmpty(sysInfo.SystemInformation);
    }

    [Fact]
    public void Test_WinSystemInfo_Logo()
    {
        SystemInfo sysInfo = new WinSystemInfo();
        Assert.NotNull(sysInfo.Logo);
        Assert.NotEmpty(sysInfo.Logo);
    }


    [Fact]
    public void Test_WinSystemInfo_SysInfo()
    {
        SystemInfo systemInfo = new WinSystemInfo();
        systemInfo.Fetch(new string[] { });
        var props = new string[] { "OS", "Host", "Kernel", "Motherboard", "Uptime", "Shell", "Resolution", "CPU", "GPU", "Memory", "Disk" };

        foreach (var prop in props)
        {
            Assert.NotNull(systemInfo.SystemInformation[prop]);
            Assert.NotEmpty(systemInfo.SystemInformation[prop]);
        }

    }

}