public interface ISystemInfo
{
    Dictionary<string, string> sysInfo { get; }
    void Fetch(string[] args);
    string ColorBlock { get; }
}