using DictionaryService.Dictionaries;
using DictionaryService.FuncClasses;
using DictionaryService.Interfaces;
using Serilog;

namespace DictionaryService.DictManagement;

public static class DictCompare
{
    public static List<IDictionary> Compare()
    {
        var resultList = new List<IDictionary>();
        var ftpList = GetLatestDictsList();
        var dbList = DbConnection.GetDictsDataFromDB();

        Log.Information("Comparing dictionaries started");

        ftpList.Sort((firstDict, secondDict) => firstDict.DictName.CompareTo(secondDict.DictName));
        dbList.Sort((firstDict, secondDict) => firstDict.DictName.CompareTo(secondDict.DictName));

        for (int i = 0; i < ftpList.Count; i++)
        {
            resultList.Add(ftpList[i]);

            if (ftpList[i].DictVersion != dbList[i].DictVersion)
                resultList[i].UpdateNeeded = true;
        }

        Log.Information("Comparing dictionaries finished");

        return resultList;
    }

    private static List<IDictionary> GetLatestDictsList()
    {
        var latestDictsList = new List<IDictionary>();

        Log.Information("Dictionaries list downloading started");

        foreach (var dictionary in ApiDictionary.ApiDictList)
        {
            switch (dictionary.DictName)
            {
                case "RPL":
                    latestDictsList.Add(new ApiDictionary(dictionary.DictName, dictionary.DownloadPath, dictionary.DownloadLink, StripPatterns.GetApiDictDate(ApiDownloader.GetApiFileName("RPL"))));
                    break;

                case "LSF":
                    latestDictsList.Add(new ApiDictionary(dictionary.DictName, dictionary.DownloadPath, dictionary.DownloadLink, StripPatterns.GetApiDictDate(ApiDownloader.GetApiFileName("LSF"))));
                    break;

                default:
                    latestDictsList.Add(new ApiDictionary(dictionary.DictName, dictionary.DownloadPath, dictionary.DownloadLink, StripPatterns.GetFtpDictVersion(FtpConfiguration.GetFullDictName(dictionary.DictName))));
                    break;
            }
        }

        foreach (var dictionary in FtpDictionary.FtpDictList)
        {
            latestDictsList.Add(new FtpDictionary(dictionary.DictName, dictionary.DownloadPath, StripPatterns.GetFtpDictVersion(FtpConfiguration.GetFullDictName(dictionary.DictName))));
        }

        Log.Information("Dictionaries list downloaded");

        return latestDictsList;
    }
}