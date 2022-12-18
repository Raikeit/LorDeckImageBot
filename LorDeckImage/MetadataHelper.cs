using LorDeckImage;
using System;

namespace LorDeckImage
{
    public abstract class MetadataUrl
    {
        public abstract string Locale { get; }

        public abstract string CoreSet { get; }

        public abstract List<string> Sets { get; }
    }

    public class MetadataUrlEnUs : MetadataUrl
    {
        public override string Locale => "en_us";
        public override string CoreSet => "https://dd.b.pvp.net/latest/core-en_us.zip";
        public override List<string> Sets => new List<string> {
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

        public override List<string> Sets => new List<string> {
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
        public static void Download(MetadataUrl metadataUrl)
        {
            // TODO: metadata/ja_jp/ フォルダ化にzipファイルをダウンロード&展開する
            // TODO: 毎回ダウンロードするのは重いので、手動でアップデートしたい時だけダウンロードするようにしたい。
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
    }
}