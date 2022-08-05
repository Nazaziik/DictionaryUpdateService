using System.Text.RegularExpressions;

namespace DictionaryService.FuncClasses;

public static class StripPatterns
{
    public static string GetFtpDictVersion(string ftpDictFullName)
    {
        var rx = new Regex(@"[^^]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var matches = rx.Matches(ftpDictFullName);
        return matches[0].ToString();
    }

    public static string GetApiDictDate(string apiDictFullName)
    {
        var rx = new Regex(@"\d{8}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var matches = rx.Matches(apiDictFullName);
        return matches[0].ToString();
    }
}
