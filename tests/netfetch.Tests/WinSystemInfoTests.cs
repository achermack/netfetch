using System.Reflection;
using System.Runtime.Versioning;
using netfetch;
using Xunit;

namespace netfetch.Tests;
[SupportedOSPlatform("windows")]
public class ISystemInfoTests
{

    [Fact]
    public void Test_OS_Contains_Windows()
    {
        Assert.Contains("Windows", new WinSystemInfo().OS);
    }

}