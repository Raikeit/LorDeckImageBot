using System;
using LoRDeckCodes;

namespace LorDeckImage
{
    class Deck
    {
        public List<Card> cards;

        public Deck(String deckcode)
        {
            var cardCodeAndCounts = LoRDeckEncoder.GetDeckFromCode(deckcode);
            cards = new List<Card>();

            foreach(CardCodeAndCount cardCodeAndCount in cardCodeAndCounts)
            {
                cards.Add(new Card(cardCodeAndCount));
            }
        }

        // public Image getImage()
        // TODO: デッキ画像を生成する。
        // TealRedのデッキ画像ジェネレータのように、クラスごとの枚数やマナカーブも表示したい
        // 1枚の画像に収まるように、カードリストを2行にしたり3行にしたり、サイズを変更したりする必要がある。
    }
}