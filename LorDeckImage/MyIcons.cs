

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
        public static List<Image<Rgba32>> CardCounts = new List<Image<Rgba32>>
        {
            Image.Load<Rgba32>(Path.Combine("img", "Count1.png")),
            Image.Load<Rgba32>(Path.Combine("img", "Count2.png")),
            Image.Load<Rgba32>(Path.Combine("img", "Count3.png")),
        };

        public static void ResizeCardCountIcons(int size)
        {
            foreach (var icon in CardCounts)
            {
                icon.Mutate(x => x.Resize(size, size));
            }
        }
    }
}
