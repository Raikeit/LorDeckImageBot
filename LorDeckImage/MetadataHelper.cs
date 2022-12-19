namespace LorDeckImage
{
    using LorDeckImage;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.IO.Compression;
    using Microsoft.VisualBasic.FileIO;

    public class MetadataHelper
    {
        public static void Download(Metadata metadata)
        {
            string dirPath = Path.Combine(metadata.metadataDirPath, metadata.Locale);
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

            string extractedDirPath = Path.Combine(dirPath, metadata.CoreSetFileName);
            string downloadedZipPath = extractedDirPath + ".zip";
            DownloadFile(metadata.CoreSet, downloadedZipPath).Wait();
            ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
            File.Delete(downloadedZipPath);
            MergeSet(extractedDirPath, dirPath, metadata.Locale);

            foreach (var dataSet in metadata.SetsFileNames.Zip(metadata.Sets))
            {
                extractedDirPath = Path.Combine(dirPath, dataSet.First);
                downloadedZipPath = extractedDirPath + ".zip";
                DownloadFile(dataSet.Second, downloadedZipPath).Wait();
                ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
                File.Delete(downloadedZipPath);
                MergeSet(extractedDirPath, dirPath, metadata.Locale);
            }
        }

        public static void Download(string locale)
        {
            Download(GetMetadataUrl(locale));
        }

        public static Metadata GetMetadataUrl(string locale)
        {
            switch (locale)
            {
                case "ja_jp":
                    return new MetadataJaJp();

                case "en_us":
                    return new MetadataEnUs();

                default:
                    return new MetadataEnUs();
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