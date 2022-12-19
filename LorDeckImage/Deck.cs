namespace LorDeckImage
{
    using LoRDeckCodes;
    using LorDeckImage.Utils;
    using SixLabors;
    using SixLabors.Fonts;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Drawing;
    using SixLabors.ImageSharp.Drawing.Processing;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using System;

    public class Deck
    {
        public static int CardWidthRatio => 680;

        public static int CardHeightRatio => 1024;

        public List<Card> Cards { get; private set; }

        public int CardWidth { get; private set; }

        public int CardHeight { get; private set; }

        public int CanvasWidth { get; private set; }

        public int CanvasHeight { get; private set; }

        public int CardCountPerLine { get; private set; }

        // public int ;

        public Deck(string deckcode, Metadata metadata)
        {
            List<CardCodeAndCount> cardCodeAndCounts = LoRDeckEncoder.GetDeckFromCode(deckcode);
            this.Cards = new List<Card>();

            foreach (CardCodeAndCount cardCodeAndCount in cardCodeAndCounts)
            {
                this.Cards.Add(new Card(cardCodeAndCount, metadata));
            }

            this.SortCardList();
            this.GetFactions();

            // デフォルトの画像サイズを768とする
            int defaultCanvasWidth = 768;
            this.SetCardCanvasSize(defaultCanvasWidth);

            // カード上に表示するアイコンサイズをCanvasに合わせて変更
            MyIcons.ResizeCardCountIcons(this.CardWidth / 4);
        }

        public Image<Rgba32> GetImage()
        {
            Image<Rgba32> canvas = new Image<Rgba32>(this.CanvasWidth, this.CanvasHeight);
            canvas.Mutate(x => x.BackgroundColor(Color.Black));

            for (int i = 0; i < this.Cards.Count(); i++)
            {
                Card card = this.Cards[i];
                using (Image<Rgba32> cardImage = card.getImage(this.CardWidth, this.CardHeight))
                {
                    // TODO: デッキ画像を生成する。
                    // TealRedのデッキ画像ジェネレータのように、クラスごとの枚数やマナカーブも表示したい
                    canvas.Mutate(x =>
                    {
                        x.DrawImage(
                            cardImage,
                            new Point(this.CardWidth * (i % this.CardCountPerLine), this.CardHeight * (i / this.CardCountPerLine)),
                            1.0f);
                    });
                }
            }

            return canvas;
        }

        public void SaveImageAsPng(string path)
        {
            Image<Rgba32> deckImage = this.GetImage();
            deckImage.SaveAsPng(path);
        }

        public void SetCardCanvasSize(int canvasWidth)
        {
            int cardKindCount = this.Cards.Count();

            // 表示画像は1行10種類までとする。20種類を超えたら3行/4行表示にする。
            int threshold = 20;
            this.CardCountPerLine = 10;
            int lineCount = (int)Math.Ceiling((double)cardKindCount / this.CardCountPerLine);

            if (threshold > cardKindCount)
            {
                // 2行表示の場合
                this.CardCountPerLine = (int)Math.Ceiling((double)cardKindCount / 2.0);
                lineCount = (int)Math.Ceiling((double)cardKindCount / this.CardCountPerLine);
            }

            this.CardWidth = canvasWidth / this.CardCountPerLine;
            this.CardHeight = (int)((double)this.CardWidth / CardWidthRatio * CardHeightRatio);
            this.CanvasWidth = canvasWidth;
            this.CanvasHeight = this.CardHeight * lineCount;
        }

        private void SortCardList()
        {
            // コスト順にソート
            this.Cards.Sort((x, y) => x.Detail.cost - y.Detail.cost);

            // カードの種類順にソート
            this.Cards.Sort((x, y) => CardDetailDataHelper.GetCardTypeOrder(x.Detail) - CardDetailDataHelper.GetCardTypeOrder(y.Detail));
        }

        private List<Faction> GetFactions()
        {
            // デッキに含まれる地域とその枚数を計算する。
            // 面倒な実装＆アップデートで色々変わる
            List<Card> championCards = new List<Card>();

            // チャンピオンカードのみを抜き出す
            foreach (var card in this.Cards)
            {
                if (card.Detail.supertype == CardType.Champion.GetStringValue())
                {
                    championCards.Add(card);
                }
            }

            foreach (var card in championCards)
            {
                // card.Detail.regionRefs
            }

            return new List<Faction>();
        }
    }
}