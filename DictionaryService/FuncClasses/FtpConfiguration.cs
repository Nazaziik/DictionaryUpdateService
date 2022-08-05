using Serilog;
using System.Net;

namespace DictionaryService.FuncClasses;

public static class FtpConfiguration
{
    public static string GetFullDictName(string folderName)
    {
        var fullDictName = string.Empty;
        var request = (FtpWebRequest)WebRequest.Create(AppConfig.FtpIp + folderName + "/");

        request.EnableSsl = true;
        request.Credentials = new NetworkCredential(AppConfig.FtpUsername, AppConfig.FtpPassword);
        request.Method = WebRequestMethods.Ftp.ListDirectory;

        try
        {
            using var response = (FtpWebResponse)request.GetResponse();
            using var responseStream = response.GetResponseStream();
            using var reader = new StreamReader(responseStream);
            fullDictName = reader.ReadToEnd();
        }
        catch (WebException e)
        {
            Log.Error(((FtpWebResponse)e.Response).StatusDescription);
        }

        var allDictVersionsList = new List<string[]>();
        var allDictVersions = fullDictName.Split('\n');

        for (int i = 0; i < allDictVersions.Length - 1; i++)
            allDictVersionsList.Add(new string[] { allDictVersions[i][..^1], StripPatterns.GetFtpDictVersion(allDictVersions[i]) });

        allDictVersionsList.Sort((firstDictToCompare, secondDictToCompare) => firstDictToCompare[1].CompareTo(secondDictToCompare[1]));

        return allDictVersionsList[^1][0];
    }

    public static void DownloadDictFTP(string downloadPath, string dictName)
    {
        var fullDictFtpPath = AppConfig.FtpIp + dictName + "/" + GetFullDictName(dictName);
        var ftpRequest = (FtpWebRequest)WebRequest.Create(fullDictFtpPath);

        ftpRequest.EnableSsl = true;
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        ftpRequest.Credentials = new NetworkCredential(AppConfig.FtpUsername, AppConfig.FtpPassword);
        ftpRequest.UseBinary = true;
        ftpRequest.Proxy = null;

        try
        {
            var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            var stream = ftpResponse.GetResponseStream();
            var buffer = new byte[2048];
            var ftpFileStream = new FileStream(downloadPath + GetFullDictName(dictName), FileMode.Create);
            var ReadCount = stream.Read(buffer, 0, buffer.Length);

            while (ReadCount > 0)
            {
                ftpFileStream.Write(buffer, 0, ReadCount);
                ReadCount = stream.Read(buffer, 0, buffer.Length);
            }

            ftpFileStream.Close();
            stream.Close();
        }
        catch (WebException e)
        {
            Log.Error(((FtpWebResponse)e.Response).StatusDescription);
        }
    }
}
