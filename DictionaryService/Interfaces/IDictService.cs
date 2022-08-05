namespace DictionaryService.Interfaces;

public interface IDictService
{
    string GetDictsToUpdate();

    void PostDicts(string dictsToUpdateJson);
}
