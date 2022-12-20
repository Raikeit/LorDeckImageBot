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
        Targon = 9,
        Shurima = 7,
        BandleCity = 10,
        Runeterra = 12,
        Unknown = 99,
    }

    public class Faction
    {
        public FactionType Type { get; private set; }

        public int Count { get; set; }

        public Faction(FactionType type, int count)
        {
            this.Type = type;
            this.Count = count;
        }
    }

    public class DeckFactions
    {
        public List<Faction> Factions { get; private set; }

        public DeckFactions()
        {
            this.Factions = new List<Faction>();
        }

        public Faction? Get(FactionType type)
        {
            foreach (var faction in this.Factions)
            {
                if (faction.Type == type) return faction;
            }

            return null;
        }

        public void Add(FactionType type, int count)
        {
            foreach (var faction in this.Factions)
            {
                if (faction.Type == type)
                {
                    faction.Count += count;
                    return;
                }
            }

            this.Factions.Add(new Faction(type, count));
        }

        public void Remove(FactionType type)
        {
            foreach (var faction in this.Factions)
            {
                if (faction.Type == type)
                {
                    this.Factions.Remove(faction);
                    return;
                }
            }
        }


        public void SortAscendingOrder()
        {
            this.Factions.Sort((x, y) => y.Count - x.Count);
        }

        public List<FactionType> GetFactionTypes()
        {
            List<FactionType> factionTypes = new List<FactionType>();

            foreach (var faction in this.Factions)
            {
                factionTypes.Add(faction.Type);
            }

            return factionTypes;
        }
    }

    public class FactionHelper
    {
        public static FactionType ConverFromString(string regionRef)
        {
            switch (regionRef)
            {
                case "Demacia":
                    return FactionType.Demacia;

                case "Freljord":
                    return FactionType.Freljord;

                case "Ionia":
                    return FactionType.Ionia;

                case "Noxus":
                    return FactionType.Noxus;

                case "PiltoverZaun":
                    return FactionType.PiltoverZaun;

                case "ShadowIsles":
                    return FactionType.ShadowIsles;

                case "Bilgewater":
                    return FactionType.Bilgewater;

                case "Targon":
                    return FactionType.Targon;

                case "Shurima":
                    return FactionType.Shurima;

                case "BandleCity":
                    return FactionType.BandleCity;

                case "Runeterra":
                    return FactionType.Runeterra;

                default:
                    return FactionType.Unknown;
            }
        }
    }
}
