using System.Drawing;
using Pastel;
namespace netfetch.util
{
    public abstract class SystemInfo
    {
        public abstract Dictionary<string, string> SystemInformation { get; }
        public abstract void Fetch(string[] args);
        protected string ColorBlock
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
            set { }
        }
        public abstract string Logo { get; }
    }
}
