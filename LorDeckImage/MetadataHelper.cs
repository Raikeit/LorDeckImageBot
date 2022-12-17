using LorDeckImage;
using System;

namespace LorDeckImage
{
    public abstract class MetadataUrl
    {
        public abstract String locale { get; }
        public abstract String coreSet { get; }
        public abstract List<String> sets { get; }
    }

    public class MetadataUrlEnUs : MetadataUrl
    {
        public override String locale => "en_us";
        public override String coreSet => "https://dd.b.pvp.net/latest/core-en_us.zip";
        public override List<String> sets => new List<string> {
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
        public override String locale => "ja_jp";
        public override String coreSet => "https://dd.b.pvp.net/latest/core-ja_jp.zip";
        public override List<String> sets => new List<string> {
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
        public static void download(MetadataUrl metadataUrl){
            // TODO: metadata/ja_jp/ フォルダ化にzipファイルをダウンロード&展開する
        }
    }
}