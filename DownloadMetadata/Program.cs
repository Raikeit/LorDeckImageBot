using LorDeckImage;
using System.Configuration;

namespace DownloadMetadata
{
    class Program
    {
        static void Main()
        {
            new MetadataHelper("ja_jp").Download();

            Dictionary<string, string> SetConfig = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                SetConfig.Add(key, ConfigurationManager.AppSettings[key]);
            }

            string locale = SetConfig["Locale"];
            if (locale != null && locale != "ja_jp")
            {
                new MetadataHelper(locale).Download();
            }
        }
    }
}