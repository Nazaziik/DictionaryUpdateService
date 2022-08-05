using DictionaryService.Dictionaries;
using DictionaryService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DictionaryUpdate.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DictionaryController : ControllerBase
{
    private readonly IDictService dictService;

    public DictionaryController(IDictService dictService)
    {
        this.dictService = dictService;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        Log.Information("Dicts GET triggered");

        string jsonDictsToUpdate = dictService.GetDictsToUpdate();

        return Ok(jsonDictsToUpdate);
    }

    [HttpPost]
    public async Task<OkResult> Post(string apiDictsToUpdateJson, string ftpDictsToUpdateJson)
    {
        Log.Information("Dict POST triggered");

        dictService.PostDicts(apiDictsToUpdateJson, ftpDictsToUpdateJson);

        return Ok();
    }
}
