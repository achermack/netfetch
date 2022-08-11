using System.Collections.ObjectModel;
using System.CommandLine;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Runtime.Versioning;
using netfetch.util;
using Pastel;

namespace netfetch.Windows
{

    [SupportedOSPlatform("Windows")]
    public class WinSystemInfo : SystemInfo
    {
        public override ReadOnlyDictionary<string, string> SystemInformation { get; }
        public WinSystemInfo()
        {
            SystemInformation = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>()
                {
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
                }
            );
        }

        private Dictionary<string, string> MakeSystemManagementQuery(string ClassName, params string[] properties)
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

        public override string Logo
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
        public override Color LogoColor => Color.DarkCyan;


        public override Color PrimaryColor => Color.Goldenrod;


        public override Color SecondaryColor => Color.GhostWhite;


        private string Disk
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
        private string Memory
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
        private string GPU
        {
            get
            {
                var gpuProps = MakeSystemManagementQuery("Win32_VideoController", "Name");
                return gpuProps["Name"];
            }
        }
        private string CPU
        {
            get
            {
                var cpuProps = MakeSystemManagementQuery("Win32_Processor", "Name");
                return cpuProps["Name"];
            }
        }
        private string Terminal
        {
            get
            {
                return Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe";
            }
        }
        private string Resolution
        {
            get
            {

                var resProps = MakeSystemManagementQuery("Win32_VideoController", "CurrentHorizontalResolution", "CurrentVerticalResolution");
                return $"{resProps["CurrentHorizontalResolution"]}x{resProps["CurrentVerticalResolution"]}";
            }
        }
        private string Shell
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

        private string Uptime
        {
            get
            {
                return TimeSpan.FromMilliseconds(Environment.TickCount).ToHumanTimeString();
            }
        }


        private string Motherboard
        {
            get
            {
                var motherboardProps = MakeSystemManagementQuery("Win32_BaseBoard", "Manufacturer", "Product");
                return $"{motherboardProps["Manufacturer"]} {motherboardProps["Product"]}";
            }
        }

        private string Kernel => Environment.OSVersion.Version.ToString();

        private string Host => System.Environment.MachineName;

        private string OS
        {
            get
            {
                var osProps = MakeSystemManagementQuery("Win32_OperatingSystem", "Caption");
                return osProps["Caption"];
            }
        }
    }
}
