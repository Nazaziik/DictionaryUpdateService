using DictionaryService.FuncClasses;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Xml;

namespace DictionaryService.DictManagement;

public static class DictUpdate
{
    public static void UpdateICD_9(string downloadPath, string dictName)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateICD_9DBProcedure(sqlXml);
    }

    public static void UpdateMODTAR(string downloadPath, string dictName)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateModtarDBProcedure(sqlXml);
    }

    public static void UpdateICD_10(string downloadPath, string dictName, string dictVersion)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateICD_10DBProcedure(sqlXml, dictVersion);
    }

    public static void UpdateGRUPPERS(string downloadPath, string dictName)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateGruppersDBProcedure(sqlXml);
    }

    public static void UpdateSLORT(string downloadPath, string dictName)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateSlortDBProcedure(sqlXml);
    }

    public static void UpdateSP_ROZ(string downloadPath, string dictName)
    {
        var sqlXml = UpdateDictTemplate(downloadPath, dictName);
        DbConnection.UpdateSp_RozDBProcedure(sqlXml);
    }

    public static void UpdateBazyl(string downloadPath, string dictName)
    {
        DbConnection.DelBAZYLDBProcedure();
        FtpConfiguration.DownloadDictFTP(downloadPath, dictName);
        UnzipFile(downloadPath, dictName);

        string[] TempDictNames = new string[]
        {
            "PRODUCEN",
            "PODSTAW",
            "SKBAZ",
            "USUNIETE",
            "CIAGODPO",
            "INTERNAZ",
            "KONCERNY",
            "KRAJE",
            "POSTAC",
            "PRZECH",
            "SYNONIMY",
            "INFORMAC",
            "KONTAKT",
            "PARAMETR",
            "CH_PRZEW",
            "TRANODPL",
            "TRANCHOR",
            "TRANKIER",
            "WERSJA",
            "BAZYL3"
        };

        foreach (var dbfFile in TempDictNames)
        {
            Console.WriteLine(dbfFile);
            var listOfDataXML = DbConnection.GetXmlFromDBFProvider(downloadPath, dbfFile);

            foreach (var dataXML in listOfDataXML)
            {
                var doc = new XmlDocument();
                doc.LoadXml(dataXML);
                using var sw = new StringWriter();
                using var xw = new XmlTextWriter(sw);
                doc.WriteTo(xw);
                using var transactionXml = new StringReader(sw.ToString());
                using var xmlReader = new XmlTextReader(transactionXml);
                var sqlXml = new SqlXml(xmlReader);
                DbConnection.UpdateBAZYLDBProcedure(sqlXml, dbfFile);
            }
        }
    }

    private static SqlXml UpdateDictTemplate(string downloadPath, string dictName)
    {
        FtpConfiguration.DownloadDictFTP(downloadPath, dictName);
        var doc = new XmlDocument();
        doc.Load(downloadPath + FtpConfiguration.GetFullDictName(dictName));
        using var sw = new StringWriter();
        using var xw = new XmlTextWriter(sw);
        doc.WriteTo(xw);
        using var transactionXml = new StringReader(sw.ToString());
        using var xmlReader = new XmlTextReader(transactionXml);
        return new SqlXml(xmlReader);
    }

    private static void UnzipFile(string downloadPath, string dictName)
    {
        var zipFullName = downloadPath + FtpConfiguration.GetFullDictName(dictName);
        var unzipPath = downloadPath;
        var zPath = "7zip/7za.exe";
        var pro = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = zPath,
            Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", zipFullName, unzipPath)
        };
        var x = Process.Start(pro);
        x.WaitForExit();
    }
}
