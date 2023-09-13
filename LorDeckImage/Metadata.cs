namespace LorDeckImage
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Text;
    using System.Text.Json;

    public class Metadata
    {
        public static string MetadataDirPath => "metadata";

        public static string CoreSetFileName => "core";

        public static List<string> SetsFileNames => new List<string>
        {
            "set1", "set2", "set3", "set4", "set5", "set6", "set6cde", "set7", "set7b", "set8"
        };

        public string Locale { get; private set; }

        public string CardImgDirPath { get; private set; }

        public string ImgExt => "png";

        public List<CardDetailData> CardDetailDatas { get; private set; }

        public Metadata(string locale)
        {
            this.Locale = locale;
            this.CardImgDirPath = Path.Combine(MetadataDirPath, this.Locale, "img", "cards");
            this.CardDetailDatas = new List<CardDetailData>();

            foreach (string setName in SetsFileNames)
            {
                // TODO: ソートの際の日本語以外の言語に対応したい。
                // 暫定対応として、読み込むJsonファイルのパスを常に日本語ファイルにする。
                // string setPath = Path.Combine(metadataDirPath, this.Locale, "data", $"{setName}-{this.Locale}.json");
                string setPath = Path.Combine(MetadataDirPath, this.Locale, "data", $"{setName}-ja_jp.json");

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
}
