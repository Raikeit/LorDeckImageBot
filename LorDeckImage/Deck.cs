using System;
using LoRDeckCodes;

namespace LorDeckImage
{
    class Deck
    {
        public Deck(String deckcode)
        {
            var cardCodeAndCounts = LoRDeckEncoder.GetDeckFromCode(encoded);
            var cards = new List<Card>();

            foreach(CardCodeAndCount cardCodeAndCount in cardCodeAndCounts)
            {
                cards.Add(new Card(cardCodeAndCount));
            }
        }
    }
}