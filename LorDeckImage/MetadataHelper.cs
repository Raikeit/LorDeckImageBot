namespace LorDeckImage
{
    using LorDeckImage;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.IO.Compression;

    public abstract class MetadataUrl
    {
        public string CoreSetFileName => "core";

        public List<string> SetsFileNames => new List<string>
        {
            "set1", "set2", "set3", "set4", "set5", "set6", "set6cde"
        };

        public abstract string Locale { get; }

        public abstract string CoreSet { get; }

        public abstract List<string> Sets { get; }
    }

    public class MetadataUrlEnUs : MetadataUrl
    {
        public override string Locale => "en_us";
        public override string CoreSet => "https://dd.b.pvp.net/latest/core-en_us.zip";
        public override List<string> Sets => new List<string>
        {
            "https://dd.b.pvp.net/latest/set1-en_us.zip",
            "https://dd.b.pvp.net/latest/set2-en_us.zip",
            "https://dd.b.pvp.net/latest/set3-en_us.zip",
            "https://dd.b.pvp.net/latest/set4-en_us.zip",
            "https://dd.b.pvp.net/latest/set5-en_us.zip",
            "https://dd.b.pvp.net/latest/set6-en_us.zip",
            "https://dd.b.pvp.net/latest/set6cde-en_us.zip"
        };
    }

    public class MetadataUrlJaJp : MetadataUrl
    {
        public override string Locale => "ja_jp";

        public override string CoreSet => "https://dd.b.pvp.net/latest/core-ja_jp.zip";

        public override List<string> Sets => new List<string>
        {
            "https://dd.b.pvp.net/latest/set1-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set2-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set3-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set4-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set5-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set6-ja_jp.zip",
            "https://dd.b.pvp.net/latest/set6cde-ja_jp.zip"
        };
    }

    class MetadataHelper
    {
        private static string metadataDirPath = "metadata";

        public static void Download(MetadataUrl metadataUrl)
        {
            string dirPath = Path.Combine(MetadataHelper.metadataDirPath, metadataUrl.Locale);
            if (!Directory.Exists(dirPath))
            {
                new DirectoryInfo(dirPath).Create();
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                di.Delete(true);
                di.Create();
            }

            string extractedDirPath = Path.Combine(dirPath, metadataUrl.CoreSetFileName);
            string downloadedZipPath = extractedDirPath + ".zip";
            MetadataHelper.DownloadFile(metadataUrl.CoreSet, downloadedZipPath).Wait();
            ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
            File.Delete(downloadedZipPath);

            foreach (var dataSet in metadataUrl.SetsFileNames.Zip(metadataUrl.Sets))
            {
                extractedDirPath = Path.Combine(dirPath, dataSet.First);
                downloadedZipPath = extractedDirPath + ".zip";
                MetadataHelper.DownloadFile(dataSet.Second, downloadedZipPath).Wait();
                ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
                File.Delete(downloadedZipPath);
            }


        }

        public static void Download(string locale)
        {
            switch (locale)
            {
                case "ja_jp":
                    MetadataHelper.Download(new MetadataUrlJaJp());
                    break;

                case "en_us":
                    MetadataHelper.Download(new MetadataUrlEnUs());
                    break;

                default:
                    MetadataHelper.Download(new MetadataUrlEnUs());
                    break;
            }
        }

        private static async Task DownloadFile(string url, string downloadPath)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var outStream = File.Create(downloadPath);
            stream.CopyTo(outStream);
        }
    }
}