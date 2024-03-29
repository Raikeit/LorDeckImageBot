﻿namespace LorDeckImage
{
    using LorDeckImage.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum CardType
    {
        // TODO: 日本語以外の言語に対応したい。暫定対応として、読み込むJsonファイルのパスを常に日本語ファイルにする。
        // 暫定対応はMetadata.csのコンストラクタある。
        [StringValue("チャンピオン")]
        Champion = 0,

        [StringValue("ユニット")]
        Unit = 1,

        [StringValue("スペル")]
        Spell = 2,

        [StringValue("ランドマーク")]
        Landmark = 3,

        [StringValue("武具")]
        Equipment = 4,
    }

    public class CardDetailData
    {
        public List<string> associatedCards { get; set; }

        public List<string> associatedCardRefs { get; set; }

        public List<Asset> assets { get; set; }

        public List<string> regions { get; set; }

        public List<string> regionRefs { get; set; }

        public int attack { get; set; }

        public int cost { get; set; }

        public int health { get; set; }

        public string description { get; set; }

        public string descriptionRaw { get; set; }

        public string levelupDescription { get; set; }

        public string levelupDescriptionRaw { get; }

        public string flavorText { get; set; }

        public string artistName { get; set; }

        public string name { get; set; }

        public string cardCode { get; set; }

        public List<string> keywords { get; set; }

        public List<string> keywordRefs { get; set; }

        public string spellSpeed { get; set; }

        public string spellSpeedRef { get; set; }

        public string rarity { get; set; }

        public string rarityRef { get; set; }

        public List<string> subtypes { get; set; }

        public string supertype { get; set; }

        public string type { get; set; }

        public bool collectible { get; set; }

        public string set { get; set; }
    }

    public class Asset
    {
        public string gameAbsolutePath { get; set; }

        public string fullAbsolutePath { get; set; }
    }

    public class CardDetailDataHelper
    {
        public static int GetCardTypeOrder(CardDetailData cardDetailData)
        {
            CardType cardType = ConverFromString(cardDetailData.type, cardDetailData.supertype);

            switch (cardType)
            {
                case CardType.Champion:
                    return 0;

                case CardType.Unit:
                    return 1;

                case CardType.Spell:
                    return 2;

                case CardType.Landmark:
                    return 3;

                case CardType.Equipment:
                    return 4;

                default:
                    return 5;
            }
        }

        public static CardType ConverFromString(string cardTypeStr, string supertype)
        {
            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                if (cardTypeStr == type.GetStringValue())
                {
                    if (type == CardType.Unit && supertype == CardType.Champion.GetStringValue())
                    {
                        return CardType.Champion;
                    }

                    return type;
                }
            }

            throw new Exception("不明なカードタイプのカードがあります。 :" + cardTypeStr + ", " + supertype);
        }
    }
}
