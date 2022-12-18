namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestDeck
    {
        [Fact]
        public void TestConstructor()
        {
            string deckcode = "CEBQCAIBEIBACBI6GEDQMBIMBUHBAHBAEYBQCAIFFAAQIAIMAIAQCAZKAIAQCBIZAECACCQ";

            Metadata metadata = MetadataHelper.GetMetadataUrl("ja_jp");
            Deck deck = new Deck(deckcode, metadata);

            Assert.NotNull(deck);
        }
    }
}