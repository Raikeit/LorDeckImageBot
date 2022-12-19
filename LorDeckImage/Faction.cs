namespace LorDeckImage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum FactionType
    {
        Demacia = 0,
        Freljord = 1,
        Ionia = 2,
        Noxus = 3,
        PiltoverZaun = 4,
        ShadowIsles = 5,
        Bilgewater = 6,
        MountTargon = 9,
        Shurima = 7,
        BandleCity = 10,
        Runeterra = 12,
    }

    public class Faction
    {
        public FactionType Type { get; private set; }

        public int Count { get; private set; }

        public Faction(FactionType type, int count)
        {
            this.Type = type;
            this.Count = count;
        }
    }
}
