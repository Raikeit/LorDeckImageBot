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
    }
}