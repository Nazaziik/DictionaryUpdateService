using DictionaryService.Dictionaries;
using DictionaryService.DictManagement;
using DictionaryService.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DictionaryService.Services;

public class DictService : IDictService
{
    public DictService()
    {

    }

    public string GetDictsToUpdate()
    {
        string dictsToUpdateJson = JsonConvert.SerializeObject(DictCompare.Compare());
        return dictsToUpdateJson;
    }

    public void PostDicts(string dictsToUpdateJson)
    {
        List<IDictionary>? dictsToUpdate = JsonConvert.DeserializeObject<List<IDictionary>>(dictsToUpdateJson);

        foreach (var dictionary in dictsToUpdate)
        {
            if (dictionary.DictName == "LSF" || dictionary.DictName == "RPL")
                new ApiDictionary(dictionary).UpdateDictionary();
            else
                new FtpDictionary(dictionary).UpdateDictionary();
        }
    }

    public static void GetConfig()
    {
        var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        var section = config.GetSection("AppSettings");
        _ = section.Get<AppConfig>();
        var apiListSection = config.GetSection("ApiDictionaries");
        ApiDictionary.ApiDictList = apiListSection.Get(typeof(List<ApiDictionary>)) as List<ApiDictionary>;
        var ftpListSection = config.GetSection("FtpDictionaries");
        FtpDictionary.FtpDictList = ftpListSection.Get(typeof(List<FtpDictionary>)) as List<FtpDictionary>;
    }
}
