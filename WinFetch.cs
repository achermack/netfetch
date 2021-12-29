

using System;
using System.CommandLine;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public class WinFetch : IFetch
{

    public static Dictionary<string, string> sysInfo = new Dictionary<string, string>() {
        {"OS", OS},
        {"Host", Host},
        {"Kernel", Kernel},
        {"Motherboard", Motherboard},
        {"Uptime", Uptime},
        {"Shell", Shell},
        {"Resolution", Resolution}
    };
    public void Fetch(string[] args)
    {
        foreach (var kv in sysInfo)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
    }
    static string Resolution
    {
        get
        {

            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo is not null && mo["CurrentHorizontalResolution"] is not null && mo["CurrentVerticalResolution"] is not null)
                {
                    return $"{mo["CurrentHorizontalResolution"]}x{mo["CurrentVerticalResolution"]}";
                }
            }
            return "" + "x" + "";
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
            return TimeSpan.FromMilliseconds(Environment.TickCount).ToString();
        }
    }


    static string Motherboard
    {
        get
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo is not null && mo["Manufacturer"] is not null && mo["Product"] is not null)
                {
                    return $"{mo["Manufacturer"]} {mo["Product"]}"!;
                }

            }
            return "";
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
    static string OS
    {
        get
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo is not null && mo["Caption"] is not null)
                {
                    return mo["Caption"].ToString()!;
                }
            }
            return "";
        }
    }
}
