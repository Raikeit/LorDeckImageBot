namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestMetadataHelper
    {
        [Fact]
        public void TestDownloadJaJp()
        {
            MetadataHelper metadataHelper = new MetadataHelper("ja_jp");
            metadataHelper.Download();

            string corePath = Path.Combine("metadata", "ja_jp", "core");
            string set6cdePath = Path.Combine("metadata", "ja_jp", "set6cde");

            Assert.True(Directory.Exists(corePath));
            Assert.True(Directory.Exists(set6cdePath));
        }

        [Fact]
        public void TestMetadataFactory()
        {
            Metadata metadataEnUs = new Metadata("en_us");
            Metadata metadataJaJp = new Metadata("ja_jp");

            Assert.Equal("en_us", metadataEnUs.Locale);
            Assert.Equal("ja_jp", metadataJaJp.Locale);
        }
    }
}