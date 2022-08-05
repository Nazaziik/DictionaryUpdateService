using DictionaryService.FuncClasses;
using DictionaryService.Interfaces;
using Serilog;

namespace DictionaryService.Dictionaries;

public class ApiDictionary : IDictionary
{
    public static List<ApiDictionary> ApiDictList;

    private string _downloadLink;

    private string _dictName;
    public string DictName { get { return _dictName; } set { _dictName = value; } }

    private string _downloadPath;
    public string DownloadPath { get { return _downloadPath; } set { _downloadPath = value; } }

    private string _dictVersion;
    public string DictVersion { get { return _dictVersion; } set { _dictVersion = value; } }

    private bool _updateNeeded;
    public bool UpdateNeeded { get { return _updateNeeded; } set { _updateNeeded = value; } }

    private bool _autoUpdate;
    public bool AutoUpdate { get { return _autoUpdate; } set { _autoUpdate = value; } }

    public ApiDictionary(string dictName, string downloadPath, string dictVersion = "", bool updateNeeded = false, bool autoUpdate = true)
    {
        _dictName = dictName;
        _downloadPath = downloadPath;
        _dictVersion = dictVersion;
        _updateNeeded = updateNeeded;
        _autoUpdate = autoUpdate;

        if (_dictName == "LSF")
            _downloadLink = "https://api.dane.gov.pl/1.4/resources/29713,link-do-strony-listy-surowcow-farmaceutycznych-na-platformie-rejestrow-medycznych?lang=pl!";
        else if (_dictName == "RPL")
            _downloadLink = "https://api.dane.gov.pl/1.4/resources/29712,link-do-strony-rejestru-produktow-leczniczych-na-platformie-rejestrow-medycznych?lang=p";
    }

    public ApiDictionary(IDictionary dictionary)
    {
        _dictName = dictionary.DictName;
        _downloadPath = dictionary.DownloadPath;
        _dictVersion = dictionary.DictVersion;
        _updateNeeded = dictionary.UpdateNeeded;
        _autoUpdate = dictionary.AutoUpdate;

        if (_dictName == "LSF")
            _downloadLink = "https://api.dane.gov.pl/1.4/resources/29713,link-do-strony-listy-surowcow-farmaceutycznych-na-platformie-rejestrow-medycznych?lang=pl!";
        else if (_dictName == "RPL")
            _downloadLink = "https://api.dane.gov.pl/1.4/resources/29712,link-do-strony-rejestru-produktow-leczniczych-na-platformie-rejestrow-medycznych?lang=p";
    }

    public ApiDictionary() { }

    public void UpdateDictionary()
    {
        Log.Information("Downloading {name}", _dictName);

        ApiDownloader.DownloadFromApi(_downloadPath, _downloadLink);

        Log.Information("Downloading {DictName} finished", _dictName);
    }
}
