using System;
using LoRDeckCodes;

namespace LorDeckImage
{
    class Deck
    {
        public List<Card> Cards = new List<Card>();

        public Deck(string deckcode)
        {
            List<CardCodeAndCount> cardCodeAndCounts = LoRDeckEncoder.GetDeckFromCode(deckcode);

            foreach (CardCodeAndCount cardCodeAndCount in cardCodeAndCounts)
            {
                this.Cards.Add(new Card(cardCodeAndCount));
            }
        }

        // public Image getImage()
        // TODO: デッキ画像を生成する。
        // TealRedのデッキ画像ジェネレータのように、クラスごとの枚数やマナカーブも表示したい
        // 1枚の画像に収まるように、カードリストを2行にしたり3行にしたり、サイズを変更したりする必要がある。
    }
}