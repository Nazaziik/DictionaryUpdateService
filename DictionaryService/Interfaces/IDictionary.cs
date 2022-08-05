namespace DictionaryService.Interfaces;

public interface IDictionary
{
    public string DictName { get; set; }

    public string DictVersion { get; set; }

    public string DownloadPath { get; set; }

    public bool UpdateNeeded { get; set; }

    public bool AutoUpdate { get; set; }

    public void UpdateDictionary();
}