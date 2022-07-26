using System.Collections.ObjectModel;
using System.Drawing;
using Pastel;
namespace netfetch.util
{
    public abstract class SystemInfo
    {
        public abstract ReadOnlyDictionary<string, string> SystemInformation { get; }
        protected readonly string Template = "{0,-60} {1,-30}";
        public void Fetch(string[] args)
        {
            string[] LogoLines = Logo.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int offset = (LogoLines.Length - SystemInformation.Count) / 2;
            int index = 0 - offset;
            foreach (var LogoLine in LogoLines)
            {
                try
                {
                    var kvp = SystemInformation.ElementAt(index);
                    if (kvp.Key.Equals("ColorBlock"))
                    {
                        Console.WriteLine(Template, LogoLine.Pastel(LogoColor), kvp.Value);
                        index++;
                        continue;
                    }

                    Console.WriteLine(Template, LogoLine?.Pastel(LogoColor), $"{kvp.Key.Pastel(PrimaryColor)}: {kvp.Value.Pastel(SecondaryColor)}");
                }
                catch
                {
                    Console.WriteLine(Template, LogoLine?.Pastel(LogoColor), "");
                }
                index++;
            }
        }
        protected string ColorBlock
        {
            get
            {
                var s = "";
                foreach (var color in Enum.GetNames(typeof(ConsoleColor)))
                {
                    s += "███".Pastel(Color.FromName(color));
                }
                return s;
            }
            set { }
        }

        public abstract Color LogoColor { get; }
        public abstract Color PrimaryColor { get; }
        public abstract Color SecondaryColor { get; }

        public abstract string Logo { get; }
    }
}
