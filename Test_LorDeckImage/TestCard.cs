namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestCard
    {
        [Fact]
        public void TestConstructor()
        {
            string deckcode = "CEBQCAIBEIBACBI6GEDQMBIMBUHBAHBAEYBQCAIFFAAQIAIMAIAQCAZKAIAQCBIZAECACCQ";

            Metadata metadata = new Metadata("ja_jp");
            Deck deck = new Deck(deckcode, metadata);
            Card card = deck.Cards.First();

            Assert.NotNull(card);
        }
    }
}