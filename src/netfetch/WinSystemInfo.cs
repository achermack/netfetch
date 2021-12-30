using System;
using System.Collections;
using System.CommandLine;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;
using Pastel;
[SupportedOSPlatform("windows")]
public class WinSystemInfo : ISystemInfo
{

    public static Dictionary<string, string> sysInfo = new Dictionary<string, string>() {
        {"OS", OS},
        {"Host", Host},
        {"Kernel", Kernel},
        {"Motherboard", Motherboard},
        {"Uptime", Uptime},
        {"Shell", Shell},
        {"Resolution", Resolution},
        {"Terminal", Terminal},
        {"CPU", CPU},
        {"GPU", GPU},
        {"Memory", Memory},
        {"Disk", Disk}
    };

    static Dictionary<string, string> MakeSystemManagementQuery(string ClassName, params string[] properties)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + ClassName);
        foreach (ManagementObject queryObj in searcher.Get())
        {
            foreach (string property in properties)
            {
                if (queryObj[property] is not null && !result.ContainsKey(property))
                {
                    result.Add(property, queryObj[property].ToString()!);
                }
            }
        }
        return result;
    }

    string logo = @"
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll

    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    lllllllllllllll   lllllllllllllll
    ";

    public void Fetch(string[] args)
    {
        string[] lines = logo.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        int offset = (lines.Length - sysInfo.Count) / 2;
        int index = 0 - offset;
        foreach (string line in lines)
        {
            try
            {
                var kvp = sysInfo.ElementAt(index);
                //Console.Write($"\t{line.Trim()}\t {kvp.Key}: {kvp.Value}");
                if (String.IsNullOrWhiteSpace(line))
                {
                    Console.Write("\t\t\t\t");
                }
                Console.WriteLine($"{line.Pastel(Color.DarkCyan)}\t {kvp.Key.Pastel(Color.Goldenrod)}: {kvp.Value}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{line.Pastel(Color.DarkCyan)}");
                //Console.WriteLine(e.Message);
            }
            index++;
        }

    }
    static string Disk
    {
        get
        {
            var diskProps = MakeSystemManagementQuery("Win32_LogicalDisk", "Size", "FreeSpace");
            double total = Convert.ToDouble(diskProps["Size"]);
            double free = Convert.ToDouble(diskProps["FreeSpace"]);
            double used = total - free;
            return $"{used / 1024 / 1024 / 1024:0.00} GB / {total / 1024 / 1024 / 1024:0.00} GB";
        }
    }
    static string Memory
    {
        get
        {
            var memProps = MakeSystemManagementQuery("Win32_OperatingSystem", "TotalVisibleMemorySize", "FreePhysicalMemory");
            double total = Convert.ToDouble(memProps["TotalVisibleMemorySize"]);
            double free = Convert.ToDouble(memProps["FreePhysicalMemory"]);
            double used = total - free;
            return $"{used / 1024 / 1024:0.00} MB / {total / 1024 / 1024:0.00} MB";
        }
    }
    static string GPU
    {
        get
        {
            var gpuProps = MakeSystemManagementQuery("Win32_VideoController", "Name");
            return gpuProps["Name"];
        }
    }
    static string CPU
    {
        get
        {
            var cpuProps = MakeSystemManagementQuery("Win32_Processor", "Name");
            return cpuProps["Name"];
        }
    }
    static string Terminal
    {
        get
        {
            return Environment.GetEnvironmentVariable("ComSpec")!;
        }
    }
    static string Resolution
    {
        get
        {

            var resProps = MakeSystemManagementQuery("Win32_VideoController", "CurrentHorizontalResolution", "CurrentVerticalResolution");
            return $"{resProps["CurrentHorizontalResolution"]}x{resProps["CurrentVerticalResolution"]}";
        }
    }
    static string Shell
    {
        get
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "pwsh",
                    Arguments = "--noprofile -c write-output $host.version.tostring()",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }

            };
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd().Trim();
        }
    }

    static string Uptime
    {
        get
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount).ToHumanTimeString();
        }
    }


    static string Motherboard
    {
        get
        {
            var motherboardProps = MakeSystemManagementQuery("Win32_BaseBoard", "Manufacturer", "Product");
            return $"{motherboardProps["Manufacturer"]} {motherboardProps["Product"]}";
        }
    }

    static string Kernel
    {
        get
        {
            return Environment.OSVersion.Version.ToString();
        }
    }
    static string Host
    {
        get
        {
            return System.Environment.MachineName;
        }

    }
    public static string OS
    {
        get
        {
            var osProps = MakeSystemManagementQuery("Win32_OperatingSystem", "Caption");
            return osProps["Caption"];
        }
    }


}
public static class ExtensionMethods
{
    public static string ToHumanTimeString(this TimeSpan span)
    {
        string[] timeSpanStrings = { span.Days > 0 ? $"{span.Days} days" : "", span.Hours > 0 ? $"{span.Hours} hours" : "", span.Minutes > 0 ? $"{span.Minutes} minutes" : "", span.Seconds > 0 ? $"{span.Seconds} seconds" : "" };
        return string.Join(", ", timeSpanStrings.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
