namespace LorDeckImage
{
    using LoRDeckCodes;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    public class Card
    {
        private string code { get; set; }

        public int Count { get; set; }

        public string ImgPath { get; set; }

        public CardDetailData? Detail { get; private set; }

        public Card(CardCodeAndCount cardCodeAndCount, Metadata metadata)
        {
            this.code = cardCodeAndCount.CardCode;
            this.Count = cardCodeAndCount.Count > 3 ? 3 : cardCodeAndCount.Count;
            this.ImgPath = Path.Combine(metadata.CardImgDirPath, this.code + "." + metadata.ImgExt);
            this.Detail = metadata.GetCardDetail(this.code);
        }

        public Image<Rgba32> getImage(int width, int height)
        {
            Image<Rgba32> image = (Image<Rgba32>)Image.Load(this.ImgPath);
            image.Mutate(x =>
            {
                x.Resize(width, height);
                x.DrawImage(
                    MyIcons.CardCounts[this.Count - 1],
                    new Point(width - (width / 4), 0),
                    1.0f);

            });

            return image;
        }
    }
}