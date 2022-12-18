namespace LorDeckImage
{
    using LoRDeckCodes;

    class Card
    {
        private string code;
        public int Count;

        public Card(CardCodeAndCount cardCodeAndCount)
        {
            this.code = cardCodeAndCount.CardCode;
            this.Count = cardCodeAndCount.Count;
            // TODO: カード画像へのリンクを持つ
        }

        // public Image getImage()
        // TODO: カード1枚画像を生成する。
        // TealRedのデッキ画像ジェネレータのように、右上に枚数も表示したい
        // OpenCVを使う？Linuxで動かすときに面倒なビルド＆デプロイ作業はしたくない
    }
}