namespace LorDeckImage
{
    using LoRDeckCodes;
    using LorDeckImage.Utils;
    using Microsoft.VisualBasic;
    using SixLabors;
    using SixLabors.Fonts;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Drawing;
    using SixLabors.ImageSharp.Drawing.Processing;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Runtime.ConstrainedExecution;
    using static System.Net.Mime.MediaTypeNames;

    public class Deck
    {
        public static int CardWidthRatio => 680;

        public static int CardHeightRatio => 1024;

        public static int DisplayMaxManaCost => 8;

        public List<Card> Cards { get; private set; }

        public int CardWidth { get; private set; }

        public int CardHeight { get; private set; }

        public int CanvasWidth { get; private set; }

        public int CanvasHeight { get; private set; }

        public int CanvasHeaderHeight { get; private set; }

        public int CardCountPerLine { get; private set; }

        public DeckFactions Factions { get; private set; }

        public static int MaxContainFactions => 2;

        public string fontName { get; private set; }

        public Deck(string deckcode, Metadata metadata)
        {
            List<CardCodeAndCount> cardCodeAndCounts = LoRDeckEncoder.GetDeckFromCode(deckcode);
            this.Cards = new List<Card>();

            foreach (CardCodeAndCount cardCodeAndCount in cardCodeAndCounts)
            {
                this.Cards.Add(new Card(cardCodeAndCount, metadata));
            }

            this.SortCardList();
            this.Factions = this.GetFactions();

            // デフォルトの画像サイズを768とする
            int defaultCanvasWidth = 768;
            this.SetCardCanvasSize(defaultCanvasWidth);

            // カード上に表示するアイコンサイズをCanvasに合わせて変更
            if (MyIcons.BaseCircleSize != this.CanvasWidth / 20) MyIcons.ResizeBaseCircleIcons(this.CanvasWidth / 20);
            if (MyIcons.CardCountIconsSize != this.CardWidth / 4) MyIcons.ResizeCardCountIcons(this.CardWidth / 4);
            if (MyIcons.RegionIconsSize != this.CanvasWidth / 8) MyIcons.ResizeRegionIcons(this.CanvasWidth / 8);
            if (MyIcons.CardTypeIconsSize != this.CanvasWidth / 10) MyIcons.ResizeCardTypeIcons(this.CanvasWidth / 10);

            Dictionary<string, string> SetConfig = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                SetConfig.Add(key, ConfigurationManager.AppSettings[key]);
            }

            this.fontName = SetConfig["Font"];
        }

        public Image<Rgba32> GetImage()
        {
            Image<Rgba32> canvas = new Image<Rgba32>(this.CanvasWidth, this.CanvasHeight);
            canvas.Mutate(x => x.BackgroundColor(Color.Black));

            FontFamily fontFamily;
            if (!SystemFonts.TryGet(this.fontName, out fontFamily))
            {
                throw new Exception($"Couldn't find font {this.fontName}");
            }

            float cardCountFontSize = (float)(MyIcons.BaseCircleSize * 0.7);
            Font cardCountFont = fontFamily.CreateFont(cardCountFontSize, FontStyle.Regular);

            // ヘッダーの描画
            // 地域アイコン
            for (int i = 0; i < this.Factions.Factions.Count(); i++)
            {
                canvas.Mutate(x =>
                {
                    x.DrawImage(
                        MyIcons.RegionIcons[this.Factions.Factions[i].Type],
                        new Point(MyIcons.RegionIconsSize * i, (this.CanvasHeaderHeight - MyIcons.RegionIconsSize) / 2),
                        1.0f);
                    x.DrawImage(
                        MyIcons.BaseCircle,
                        new Point(
                            (MyIcons.RegionIconsSize * (i + 1)) - MyIcons.BaseCircleSize,
                            MyIcons.RegionIconsSize - MyIcons.BaseCircleSize),
                        1.0f);
                    x.DrawText(
                            string.Format("{0, 2}", this.Factions.Factions[i].Count),
                            cardCountFont,
                            new Color(new Rgba32((byte)0, (byte)0, (byte)0, (byte)200)),
                            new PointF(
                                (MyIcons.RegionIconsSize * (i + 1)) - MyIcons.BaseCircleSize + MyIcons.BaseCircleSize * 0.1f,
                                MyIcons.RegionIconsSize - MyIcons.BaseCircleSize + MyIcons.BaseCircleSize * 0.1f));
                });
            }

            // カードタイプアイコン
            Dictionary<CardType, int> countCardType = this.GetCountCardType();
            {
                int i = 0;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                {
                    canvas.Mutate(x =>
                    {
                        x.DrawImage(
                            MyIcons.CardTypeIcons[cardType],
                            new Point(
                                (MyIcons.RegionIconsSize * MaxContainFactions) + (MyIcons.CardTypeIconsSize * i),
                                (this.CanvasHeaderHeight - MyIcons.CardTypeIconsSize) / 2),
                            1.0f);
                        x.DrawImage(
                            MyIcons.BaseCircle,
                            new Point(
                                (MyIcons.RegionIconsSize * MaxContainFactions) + (MyIcons.CardTypeIconsSize * (i + 1)) - MyIcons.BaseCircleSize,
                                ((this.CanvasHeaderHeight - MyIcons.CardTypeIconsSize) / 2) + MyIcons.CardTypeIconsSize - MyIcons.BaseCircleSize),
                            1.0f);
                        x.DrawText(
                            string.Format("{0, 2}", countCardType[cardType]),
                            cardCountFont,
                            new Color(new Rgba32((byte)0, (byte)0, (byte)0, (byte)200)),
                            new PointF(
                                (MyIcons.RegionIconsSize * MaxContainFactions) + (MyIcons.CardTypeIconsSize * (i + 1)) - MyIcons.BaseCircleSize + MyIcons.BaseCircleSize * 0.1f,
                                ((this.CanvasHeaderHeight - MyIcons.CardTypeIconsSize) / 2) + MyIcons.CardTypeIconsSize - MyIcons.BaseCircleSize + MyIcons.BaseCircleSize * 0.1f));
                    });

                    i++;
                }
            }

            // マナカーブ
            List<int> manaCurve = this.GetManaCurve();
            int maxNumOfManaCards = manaCurve.Max<int>();

            int startPositionX = (MyIcons.RegionIconsSize * MaxContainFactions) + (MyIcons.CardTypeIconsSize * Enum.GetNames(typeof(CardType)).Length) + 10;
            int topPositionY = this.CanvasHeaderHeight / 7;
            int numGraphs = DisplayMaxManaCost + 1;
            int spaceOfGraph = (int)((this.CanvasWidth - startPositionX) / numGraphs);
            int widthOfGraph = (int)((this.CanvasWidth - startPositionX) / numGraphs * 0.9);
            int heightOfGraph = this.CanvasHeaderHeight / 7 * 5;
            int bottomPositionY = topPositionY + heightOfGraph;

            float manaCurveFontSize = widthOfGraph * 0.7f;
            Font manaCurveFont = fontFamily.CreateFont(manaCurveFontSize, FontStyle.Regular);

            for (int i = 0; i < numGraphs; i++)
            {
                int numOfCards = manaCurve[i];
                double barLengthRatio = (double)numOfCards / (double)maxNumOfManaCards;

                canvas.Mutate(x =>
                {
                    x.FillPolygon(
                        new Color(new Rgba32((byte)39, (byte)18, (byte)125, (byte)128)),
                        new List<PointF>()
                        {
                             new PointF(startPositionX + (i * spaceOfGraph), topPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, topPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, bottomPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph), bottomPositionY),
                        }.ToArray()
                    );

                    x.FillPolygon(
                        new Color(new Rgba32((byte)100, (byte)149, (byte)237, (byte)200)),
                        new List<PointF>()
                        {
                             new PointF(startPositionX + (i * spaceOfGraph), (int)(bottomPositionY - ((bottomPositionY - topPositionY) * barLengthRatio))),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, (int)(bottomPositionY - ((bottomPositionY - topPositionY) * barLengthRatio))),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, bottomPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph), bottomPositionY),
                        }.ToArray()
                    );

                    x.DrawPolygon(
                        new Pen(new Color(new Rgba32((byte)142, (byte)113, (byte)208, (byte)128)), 1.0f),
                        new List<PointF>()
                        {
                             new PointF(startPositionX + (i * spaceOfGraph), topPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, topPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph) + widthOfGraph, bottomPositionY),
                             new PointF(startPositionX + (i * spaceOfGraph), bottomPositionY),
                        }.ToArray()
                    );

                    x.DrawText(
                            string.Format("{0, 2}", numOfCards),
                            manaCurveFont,
                            new Color(new Rgba32((byte)255, (byte)255, (byte)255, (byte)200)),
                            new PointF(startPositionX + (i * spaceOfGraph), 0.0f));

                    x.DrawText(
                            string.Format("{0, 2}", i < (numGraphs - 1) ? i.ToString() : i.ToString() + "+"),
                            manaCurveFont,
                            new Color(new Rgba32((byte)255, (byte)255, (byte)255, (byte)200)),
                            new PointF(startPositionX + (i * spaceOfGraph), bottomPositionY));
                });
            }

            // カードリストの描画
            for (int i = 0; i < this.Cards.Count(); i++)
            {
                Card card = this.Cards[i];
                using (Image<Rgba32> cardImage = card.getImage(this.CardWidth, this.CardHeight))
                {
                    canvas.Mutate(x =>
                    {
                        x.DrawImage(
                            cardImage,
                            new Point(
                                this.CardWidth * (i % this.CardCountPerLine),
                                this.CanvasHeaderHeight + (this.CardHeight * (i / this.CardCountPerLine))),
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
            this.CanvasHeaderHeight = this.CanvasWidth / 8;
            this.CanvasHeight = this.CanvasHeaderHeight + (this.CardHeight * lineCount);
        }

        public int GetDeckCardCount()
        {
            int count = 0;
            foreach (var card in this.Cards)
            {
                count += card.Count;
            }

            return count;
        }

        private void SortCardList()
        {
            // コスト順にソート
            this.Cards.Sort((x, y) => x.Detail.cost - y.Detail.cost);

            // TODO: ソート時に元の順番を維持するようにする。
            // LINQを使う？
            // カードの種類順にソート
            this.Cards.Sort((x, y) => CardDetailDataHelper.GetCardTypeOrder(x.Detail) - CardDetailDataHelper.GetCardTypeOrder(y.Detail));
        }

        private DeckFactions GetFactions()
        {
            // デッキに含まれる地域とその枚数を計算する。
            // 面倒な実装＆アップデートで色々変わる
            List<Card> championCards = new List<Card>();
            DeckFactions factions = new DeckFactions();

            foreach (var card in this.Cards)
            {
                // チャンピオンカードのみを抜き出す
                if (card.Detail.supertype == CardType.Champion.GetStringValue())
                {
                    championCards.Add(card);
                }

                // 各地域の枚数をカウントする
                foreach (var regionRef in card.Detail.regionRefs)
                {
                    factions.Add(FactionHelper.ConverFromString(regionRef), card.Count);
                }
            }

            // デュアル地域対応
            int maxDeckContainRegionKind = 2; // デッキに含まれる地域の最大数
            if (factions.Factions.Count > maxDeckContainRegionKind)
            {
                List<FactionType> containTypes = factions.GetFactionTypes();

                foreach (var card in championCards)
                {
                    foreach (var regionRef in card.Detail.regionRefs)
                    {
                        containTypes.Remove(FactionHelper.ConverFromString(regionRef));
                    }
                }

                foreach (var type in containTypes)
                {
                    factions.Remove(type);
                }

                factions.SortAscendingOrder();
                while (factions.Factions.Count > maxDeckContainRegionKind)
                {
                    factions.Factions.RemoveAt(factions.Factions.Count - 1);
                }
            }

            // ここまでで2地域まで絞ること(factions.Factions.Count <= 2)

            // ルーンテラ地域対応
            if (factions.Factions.Count == 1 && factions.Factions[0].Type == FactionType.Runeterra)
            {
                factions.Factions[0].Count = this.GetDeckCardCount();
            }
            else if (factions.Factions.Count == 2)
            {
                if (factions.Factions[0].Type == FactionType.Runeterra)
                {
                    factions.Factions[0].Count = this.GetDeckCardCount() - factions.Factions[1].Count;
                }
                else if (factions.Factions[1].Type == FactionType.Runeterra)
                {
                    factions.Factions[1].Count = this.GetDeckCardCount() - factions.Factions[0].Count;
                }
            }
            return factions;
        }

        public Dictionary<CardType, int> GetCountCardType()
        {
            Dictionary<CardType, int> countCardType = new Dictionary<CardType, int>();

            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                countCardType[cardType] = 0;
            }

            foreach (var card in this.Cards)
            {
                countCardType[CardDetailDataHelper.ConverFromString(card.Detail.type, card.Detail.supertype)] += card.Count;
            }

            return countCardType;
        }

        public List<int> GetManaCurve()
        {
            // 0マナの分を足してDisplayMaxManaCost+1個のリストを作る & 0で初期化する
            List<int> manaCurve = new int[DisplayMaxManaCost + 1].ToList();

            foreach (var card in this.Cards)
            {
                if (card.Detail.cost < DisplayMaxManaCost)
                {
                    manaCurve[card.Detail.cost] += card.Count;
                }
                else
                {
                    // DisplayMaxManaCost以上の枚数はまとめて返す
                    manaCurve[DisplayMaxManaCost] += card.Count;
                }
            }

            return manaCurve;
        }
    }
}