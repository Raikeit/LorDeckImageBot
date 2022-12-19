namespace LorDeckImage
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Text;
    using System.Text.Json;

    public abstract class Metadata
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

        public abstract string CardImgDirPath { get; }

        public string ImgExt => "png";

        public List<CardDetailData> CardDetailDatas { get; private set; }

        public Metadata()
        {
            this.CardDetailDatas = new List<CardDetailData>();

            foreach (string setName in this.SetsFileNames)
            {
                // TODO: ソートの際の日本語以外の言語に対応したい。
                // 暫定対応として、読み込むJsonファイルのパスを常に日本語ファイルにする。
                // string setPath = Path.Combine(this.metadataDirPath, this.Locale, "data", $"{setName}-{this.Locale}.json");
                string setPath = Path.Combine(this.metadataDirPath, this.Locale, "data", $"{setName}-ja_jp.json");

                string jsonString = File.ReadAllText(setPath);
                List<CardDetailData>? datas = JsonSerializer.Deserialize<List<CardDetailData>>(jsonString);

                if (datas != null)
                {
                    this.CardDetailDatas.AddRange(datas);
                }
            }
        }

        public CardDetailData? GetCardDetail(string cardCode)
        {
            return this.CardDetailDatas.Find(x => x.cardCode == cardCode);
        }
    }

    public class MetadataEnUs : Metadata
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

        public override string CardImgDirPath => Path.Combine(this.metadataDirPath, this.Locale, "img", "cards");


    }

    public class MetadataJaJp : Metadata
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

        public override string CardImgDirPath => Path.Combine(this.metadataDirPath, this.Locale, "img", "cards");
    }
}
