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
        public string Locale { get; private set; }

        public string CoreSet { get; private set; }

        public List<string> Sets { get; private set; }

        public MetadataHelper(string locale)
        {
            this.Locale = locale;
            this.CoreSet = string.Format("https://dd.b.pvp.net/latest/core-{0}.zip", locale);
            this.Sets = new List<string>
            {
                string.Format("https://dd.b.pvp.net/latest/set1-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set2-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set3-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set4-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set5-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set6-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set6cde-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set7-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set7b-lite-{0}.zip", locale),
                string.Format("https://dd.b.pvp.net/latest/set8-lite-{0}.zip", locale),
            };
        }

        public void Download()
        {
            string dirPath = Path.Combine(Metadata.MetadataDirPath, this.Locale);
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

            string extractedDirPath = Path.Combine(dirPath, Metadata.CoreSetFileName);
            string downloadedZipPath = extractedDirPath + ".zip";
            Console.WriteLine(string.Format("Downloading... {0}", downloadedZipPath));
            DownloadFile(this.CoreSet, downloadedZipPath).Wait(-1);
            Console.WriteLine(string.Format("Extracting... {0}", downloadedZipPath));
            ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
            File.Delete(downloadedZipPath);
            MergeSet(extractedDirPath, dirPath, this.Locale);

            foreach (var dataSet in Metadata.SetsFileNames.Zip(this.Sets))
            {
                extractedDirPath = Path.Combine(dirPath, dataSet.First);
                downloadedZipPath = extractedDirPath + ".zip";
                Console.WriteLine(string.Format("Downloading... {0}", downloadedZipPath));
                DownloadFile(dataSet.Second, downloadedZipPath).Wait(-1);
                Console.WriteLine(string.Format("Extracting... {0}", downloadedZipPath));
                ZipFile.ExtractToDirectory(downloadedZipPath, extractedDirPath);
                File.Delete(downloadedZipPath);
                MergeSet(extractedDirPath, dirPath, this.Locale);
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