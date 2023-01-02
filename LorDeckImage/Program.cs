using LorDeckImage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// MetadataHelper.Download("ja_jp");
// MetadataHelper.Download("abc");

string deckcode = "CQBACAIEEQBAMBAVFABQCBQKC4AQMBBAAMCQUMNGAHKQCBQBAMCAWAIEAQDQEBQKE4UAIAIEAENSCNAGAUFAIKBYL22ADRQBA4DAIBQJBYJBMIRL";

Metadata metadata = new Metadata("ja_jp");
Deck deck = new Deck(deckcode, metadata);
Card card = deck.Cards.First();

Image<Rgba32> deckImage = deck.GetImage();
deckImage.SaveAsPng("test.png");