namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestCard
    {
        [Fact]
        public void TestConstructor()
        {
            string deckcode = "CEBQCAIBEIBACBI6GEDQMBIMBUHBAHBAEYBQCAIFFAAQIAIMAIAQCAZKAIAQCBIZAECACCQ";

            Deck deck = new Deck(deckcode);
            Card card = deck.Cards.First();

            Assert.NotNull(card);
        }
    }
}