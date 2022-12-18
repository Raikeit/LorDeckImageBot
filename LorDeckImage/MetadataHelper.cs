namespace LorDeckImage
{
    using LorDeckImage;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.IO.Compression;
    using Microsoft.VisualBasic.FileIO;

    public abstract class MetadataUrl
    {
        public string metadataDirPath => "metadata";

        public string CoreSetFileName => "core";

        public List<string> SetsFileNames => new List<string>
        {
            "set1", "set2", "set3", "set4", "set5", "set6", "set6cde"
        };

        public abstract string Locale { get; }

        public abstract string CoreSet { get; }

        public abstract List<string> Sets { get; }

        public abstract string CardImgDirPath { get;  }

        public string ImgExt => "png";
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

        public override string CardImgDirPath => Path.Combine(this.metadataDirPath, this.Locale, "cards");
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

        public override string CardImgDirPath => Path.Combine(this.metadataDirPath, this.Locale, "cards");
    }

    public class MetadataHelper
    {
        public static void Download(MetadataUrl metadataUrl)
        {
            string dirPath = Path.Combine(metadataUrl.metadataDirPath, metadataUrl.Locale);
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
            DownloadFile(metadataUrl.CoreSet, downloadedZipPath).Wait();
            ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
            File.Delete(downloadedZipPath);
            MergeSet(extractedDirPath, dirPath, metadataUrl.Locale);

            foreach (var dataSet in metadataUrl.SetsFileNames.Zip(metadataUrl.Sets))
            {
                extractedDirPath = Path.Combine(dirPath, dataSet.First);
                downloadedZipPath = extractedDirPath + ".zip";
                DownloadFile(dataSet.Second, downloadedZipPath).Wait();
                ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
                File.Delete(downloadedZipPath);
                MergeSet(extractedDirPath, dirPath, metadataUrl.Locale);
            }


        }

        public static void Download(string locale)
        {
            Download(GetMetadataUrl(locale));
        }

        public static MetadataUrl GetMetadataUrl(string locale)
        {
            switch (locale)
            {
                case "ja_jp":
                    return new MetadataUrlJaJp();

                case "en_us":
                    return new MetadataUrlEnUs();

                default:
                    return new MetadataUrlEnUs();
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

        private static void MergeSet(string srcSetPath, string destPath, string locale)
        {
            // メモ：FileSystem.MoveDirectory()ってUnixで使える？
            FileSystem.MoveDirectory(Path.Combine(srcSetPath, locale, "img"), Path.Combine(destPath, "img"), true);
            FileSystem.MoveDirectory(Path.Combine(srcSetPath, locale, "data"), Path.Combine(destPath, "data"), true);
        }
    }
}