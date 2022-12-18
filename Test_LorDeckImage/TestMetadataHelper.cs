namespace Test_LorDeckImage
{
    using LorDeckImage;

    public class TestMetadataHelper
    {
        [Fact]
        public void TestDownloadJaJp()
        {
            MetadataHelper.Download("ja_jp");

            string corePath = Path.Combine("metadata", "ja_jp", "core");
            string set6cdePath = Path.Combine("metadata", "ja_jp", "set6cde");

            Assert.True(Directory.Exists(corePath));
            Assert.True(Directory.Exists(set6cdePath));
        }

        [Fact]
        public void TestMetadataFactory()
        {
            Metadata metadataEnUs = MetadataHelper.GetMetadataUrl("en_us");
            Metadata metadataJaJp = MetadataHelper.GetMetadataUrl("ja_jp");

            Assert.Equal("en_us", metadataEnUs.Locale);
            Assert.Equal("ja_jp", metadataJaJp.Locale);
        }
    }
}