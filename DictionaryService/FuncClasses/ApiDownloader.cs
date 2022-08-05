using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Net;

namespace DictionaryService.FuncClasses;

public static class ApiDownloader
{
    private static readonly string[] inJsonPath = { "data.relationships.dataset.links.related", "data.relationships.resources.links.related" };

    public static void DownloadFromApi(string downloadPath, string downloadLink)
    {
        var sourceApiDictLink = GetApiLink(downloadLink);

        try
        {
            using var wc = new WebClient();
            wc.DownloadFile(sourceApiDictLink, downloadPath + GetApiFileName(downloadLink));
        }
        catch (Exception ex)
        {
            Log.Error("Error {ex}", ex.Message);
        }
    }

    public static string GetApiFileName(string downloadLink)
    {
        var fileName = "";
        var uri = GetApiLink(downloadLink);

        try
        {
            using (var wc = new WebClient())
            {
                var data = wc.DownloadData(uri);

                if (!string.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                {
                    fileName = wc.ResponseHeaders["Content-Disposition"].Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 9).Replace("\"", "");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error {ex}", ex.Message);
        }

        return fileName;
    }

    private static string GetApiLink(string downloadLink)
    {
        string datasetLinkString = String.Empty;
        int i = 0;

        try
        {
            var apiLinkString = GetLinkFromJson(downloadLink, inJsonPath[0]);
            datasetLinkString = GetLinkFromJson(apiLinkString, inJsonPath[1]);

            var jObj = (JObject)JsonConvert.DeserializeObject(GetJson(datasetLinkString));

            for (; i < jObj.Count; i++)
            {
                var resourceAttribute = GetLinkFromJson(datasetLinkString, $"data[{i}].attributes.format");
                if (resourceAttribute == "xml")
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error {ex}", ex.Message);
        }

        return GetLinkFromJson(datasetLinkString, $"data[{i}].attributes.link");
    }

    private static string GetLinkFromJson(string address, string linkPath)
    {
        string finalAddress = String.Empty;

        try
        {
            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(address);
                var data = (JObject)JsonConvert.DeserializeObject(json);
                finalAddress = data.SelectToken(linkPath).Value<string>();
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error {ex}", ex.Message);
        }

        return finalAddress;
    }

    private static string GetJson(string address)
    {
        string jsonString = String.Empty;

        try
        {
            using (var wc = new WebClient())
                jsonString = wc.DownloadString(address);
        }
        catch (Exception ex)
        {
            Log.Error("Error {ex}", ex.Message);
        }

        return jsonString;
    }
}
