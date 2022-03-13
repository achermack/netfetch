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
    public Dictionary<string, string> sysInfo { get; }
    public WinSystemInfo()
    {
        sysInfo = new Dictionary<string, string>() {
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
        {"Disk", Disk},
        {"ColorBlock", ColorBlock}
    };
    }



    public Dictionary<string, string> MakeSystemManagementQuery(string ClassName, params string[] properties)
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
    public string logo
    {
        get
        {
            switch (OS)
            {
                case string a when a.Contains("Windows 11"):
                    return @"
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
    lllllllllllllll   lllllllllllllll";
                case string b when b.Contains("Windows 10") || b.Contains("Windows 8.1") || b.Contains("Windows 8") || b.Contains("Windows 7"):
                    return @"
                        ....,,:;+ccllll
            ...,,+:;  cllllllllllllllllll
    ,cclllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
                                        
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    llllllllllllll  lllllllllllllllllll
    ``'ccllllllllll  lllllllllllllllllll
            ``' \\*::  :ccllllllllllllllll
                            ````````''*::cl
                                        ````";
                case string c when c.Contains("Windows 7") || c.Contains("Windows Vista") || c.Contains("Windows XP"):
                    return @"
         ,.=:!!t3Z3z.,               
        :tt:::tt333EE3               
        Et:::ztt33EEE  ${e}[32m@Ee.,      ..,
       ;tt:::tt333EE7 ${e}[32m;EEEEEEttttt33#
      :Et:::zt333EEQ. ${e}[32mSEEEEEttttt33QL
      it::::tt333EEF ${e}[32m@EEEEEEttttt33F 
     ;3=*^``````'*4EEV ${e}[32m:EEEEEEttttt33@. 
     ,.=::::it=., ${e}[31m`` ${e}[32m@EEEEEEtttz33QF  
    ;::::::::zt33)   ${e}[32m'4EEEtttji3P*   
   :t::::::::tt33 ${e}[33m:Z3z..  ${e}[32m```` ${e}[33m,..g.   
   i::::::::zt33F ${e}[33mAEEEtttt::::ztF    
  ;:::::::::t33V ${e}[33m;EEEttttt::::t3     
  E::::::::zt33L ${e}[33m@EEEtttt::::z3F     
 {3=*^``````'*4E3) ${e}[33m;EEEtttt:::::tZ``     
             `` ${e}[33m:EEEEtttt::::z7       
                 'VEzjt:;;z>*``                       ";

                default:
                    return "";
            }

        }
    }

    public string ColorBlock
    {
        get
        {
            string s = "";
            foreach (var color in Enum.GetNames(typeof(ConsoleColor)))
            {
                s += "███".Pastel(Color.FromName(color));
            }
            return s;
        }
    }



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
                if (kvp.Key.Equals("ColorBlock"))
                {
                    Console.WriteLine($"{line.Pastel(Color.DarkCyan)}\t {kvp.Value}");
                    index++;
                    continue;
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
    public string Disk
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
    public string Memory
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
    public string GPU
    {
        get
        {
            var gpuProps = MakeSystemManagementQuery("Win32_VideoController", "Name");
            return gpuProps["Name"];
        }
    }
    public string CPU
    {
        get
        {
            var cpuProps = MakeSystemManagementQuery("Win32_Processor", "Name");
            return cpuProps["Name"];
        }
    }
    public string Terminal
    {
        get
        {
            return Environment.GetEnvironmentVariable("ComSpec")!;
        }
    }
    public string Resolution
    {
        get
        {

            var resProps = MakeSystemManagementQuery("Win32_VideoController", "CurrentHorizontalResolution", "CurrentVerticalResolution");
            return $"{resProps["CurrentHorizontalResolution"]}x{resProps["CurrentVerticalResolution"]}";
        }
    }
    public string Shell
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

    public string Uptime
    {
        get
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount).ToHumanTimeString();
        }
    }


    public string Motherboard
    {
        get
        {
            var motherboardProps = MakeSystemManagementQuery("Win32_BaseBoard", "Manufacturer", "Product");
            return $"{motherboardProps["Manufacturer"]} {motherboardProps["Product"]}";
        }
    }

    public string Kernel
    {
        get
        {
            return Environment.OSVersion.Version.ToString();
        }
    }
    public string Host
    {
        get
        {
            return System.Environment.MachineName;
        }

    }
    public string OS
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
