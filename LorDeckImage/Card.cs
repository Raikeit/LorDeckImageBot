namespace LorDeckImage
{
    using LoRDeckCodes;

    public class Card
    {
        private string code { get; set; }

        public int Count { get; set; }

        public string ImgPath { get; set; }

        public Card(CardCodeAndCount cardCodeAndCount, MetadataUrl metadataUrl)
        {
            this.code = cardCodeAndCount.CardCode;
            this.Count = cardCodeAndCount.Count;
            this.ImgPath = Path.Combine(metadataUrl.CardImgDirPath, this.code + "." + metadataUrl.ImgExt);
        }

        // public Image getImage()
        // TODO: カード1枚画像を生成する。
        // TealRedのデッキ画像ジェネレータのように、右上に枚数も表示したい
        // OpenCVを使う？Linuxで動かすときに面倒なビルド＆デプロイ作業はしたくない
    }
}