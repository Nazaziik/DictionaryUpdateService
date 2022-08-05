using DictionaryService.DictManagement;
using DictionaryService.FuncClasses;
using DictionaryService.Interfaces;
using Serilog;

namespace DictionaryService.Dictionaries;

public class FtpDictionary : IDictionary
{
    public static List<FtpDictionary> FtpDictList;

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

    public FtpDictionary(string dictName, string downloadPath, string dictVersion = "", bool updateNeeded = false, bool autoUpdate = true)
    {
        _dictName = dictName;
        _downloadPath = downloadPath;
        _dictVersion = dictVersion;
        _updateNeeded = updateNeeded;
        _autoUpdate = autoUpdate;
    }

    public FtpDictionary(IDictionary dictionary)
    {
        _dictName = dictionary.DictName;
        _downloadPath = dictionary.DownloadPath;
        _dictVersion = dictionary.DictVersion;
        _updateNeeded = dictionary.UpdateNeeded;
        _autoUpdate = dictionary.AutoUpdate;
    }

    public FtpDictionary() { }

    public void UpdateDictionary()
    {
        Log.Information("Downloading {name}", _dictName);

        switch (_dictName)
        {
            case "ICD_9":
                DictUpdate.UpdateICD_9(_downloadPath, _dictName);
                break;

            case "MODTAR":
                DictUpdate.UpdateMODTAR(_downloadPath, _dictName);
                break;

            case "ICD_10":
                DictUpdate.UpdateICD_10(_downloadPath, _dictName, _dictVersion);
                break;

            case "GRUPER.AMB":
                DictUpdate.UpdateGRUPPERS(_downloadPath, _dictName);
                break;

            case "GRUPER.HOSP":
                DictUpdate.UpdateGRUPPERS(_downloadPath, _dictName);
                break;

            case "SLORT":
                DictUpdate.UpdateSLORT(_downloadPath, _dictName);
                break;

            case "SP_ROZ":
                DictUpdate.UpdateSP_ROZ(_downloadPath, _dictName);
                break;

            case "BAZYL":
                DictUpdate.UpdateBazyl(_downloadPath, _dictName);
                break;

            default:
                FtpConfiguration.DownloadDictFTP(_downloadPath, _dictName);
                break;
        }

        Log.Information("Downloading {DictName} finished", _dictName);
    }
}
