namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestDeck
    {
        [Fact]
        public void TestConstructor()
        {
            string deckcode = "CEBQCAIBEIBACBI6GEDQMBIMBUHBAHBAEYBQCAIFFAAQIAIMAIAQCAZKAIAQCBIZAECACCQ";

            Deck deck = new Deck(deckcode);

            Assert.NotNull(deck);
        }
    }
}