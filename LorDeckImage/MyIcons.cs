

namespace LorDeckImage
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MyIcons
    {
        // TODO: プロジェクトに画像を含める（ビルド時に自動配置）
        public static Image<Rgba32> BaseCircle = Image.Load<Rgba32>(Path.Combine("img", "BaseCircle.png"));

        public static List<Image<Rgba32>> CardCounts = new List<Image<Rgba32>>
        {
            Image.Load<Rgba32>(Path.Combine("img", "Count1.png")),
            Image.Load<Rgba32>(Path.Combine("img", "Count2.png")),
            Image.Load<Rgba32>(Path.Combine("img", "Count3.png")),
        };

        public static Dictionary<FactionType, Image<Rgba32>> RegionIcons = new Dictionary<FactionType, Image<Rgba32>>
        {
            { FactionType.Demacia, Image.Load<Rgba32>(Path.Combine("img", "icon-demacia.png")) },
            { FactionType.Freljord, Image.Load<Rgba32>(Path.Combine("img", "icon-freljord.png")) },
            { FactionType.Ionia, Image.Load<Rgba32>(Path.Combine("img", "icon-ionia.png")) },
            { FactionType.PiltoverZaun, Image.Load<Rgba32>(Path.Combine("img", "icon-piltoverzaun.png")) },
            { FactionType.Noxus, Image.Load<Rgba32>(Path.Combine("img", "icon-noxus.png")) },
            { FactionType.Bilgewater, Image.Load<Rgba32>(Path.Combine("img", "icon-bilgewater.png")) },
            { FactionType.ShadowIsles, Image.Load<Rgba32>(Path.Combine("img", "icon-shadowisles.png")) },
            { FactionType.Targon, Image.Load<Rgba32>(Path.Combine("img", "icon-targon.png")) },
            { FactionType.BandleCity, Image.Load<Rgba32>(Path.Combine("img", "icon-bandlecity.png")) },
            { FactionType.Shurima, Image.Load<Rgba32>(Path.Combine("img", "icon-shurima.png")) },
            { FactionType.Runeterra, Image.Load<Rgba32>(Path.Combine("img", "icon-runeterra.png")) },
            { FactionType.Unknown, Image.Load<Rgba32>(Path.Combine("img", "icon-all.png")) },
        };

        public static Dictionary<CardType, Image<Rgba32>> CardTypeIcons = new Dictionary<CardType, Image<Rgba32>>
        {
            { CardType.Champion, Image.Load<Rgba32>(Path.Combine("img", "icon-champion.png")) },
            { CardType.Unit, Image.Load<Rgba32>(Path.Combine("img", "icon-follower.png")) },
            { CardType.Spell, Image.Load<Rgba32>(Path.Combine("img", "icon-spell.png")) },
            { CardType.Landmark, Image.Load<Rgba32>(Path.Combine("img", "icon-landmark.png")) },
            { CardType.Equipment, Image.Load<Rgba32>(Path.Combine("img", "icon-equipment.png")) },
        };

        public static int BaseCircleSize { get; private set; }

        public static int CardCountIconsSize { get; private set; }

        public static int RegionIconsSize { get; private set; }

        public static int CardTypeIconsSize { get; private set; }

        public static void ResizeBaseCircleIcons(int size)
        {
            BaseCircle.Mutate(x => x.Resize(size, size));
            BaseCircleSize = size;
        }

        public static void ResizeCardCountIcons(int size)
        {
            foreach (var icon in CardCounts)
            {
                icon.Mutate(x => x.Resize(size, size));
            }

            CardCountIconsSize = size;
        }

        public static void ResizeRegionIcons(int size)
        {
            foreach (FactionType factionType in Enum.GetValues(typeof(FactionType)))
            {
                RegionIcons[factionType].Mutate(x => x.Resize(size, size));
            }

            RegionIconsSize = size;
        }

        public static void ResizeCardTypeIcons(int size)
        {
            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                CardTypeIcons[cardType].Mutate(x => x.Resize(size, size));
            }

            CardTypeIconsSize = size;
        }
    }
}
