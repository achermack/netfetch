using System.Drawing;
using Pastel;
namespace netfetch.util
{
    public abstract class SystemInfo
    {
        public abstract Dictionary<string, string> SystemInformation { get; }
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
                    if (String.IsNullOrWhiteSpace(LogoLine))
                    {
                        Console.Write("\t\t\t\t");
                    }
                    if (kvp.Key.Equals("ColorBlock"))
                    {
                        Console.WriteLine($"{LogoLine.Pastel(LogoColor)}\t {kvp.Value}");
                        index++;
                        continue;
                    }
                    Console.WriteLine($"{LogoLine.Pastel(LogoColor)}\t {kvp.Key.Pastel(PrimaryColor)}: {kvp.Value.Pastel(SecondaryColor)}");

                }
                catch
                {

                    Console.WriteLine($"{LogoLine.Pastel(LogoColor)}");
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
