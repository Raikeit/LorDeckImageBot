namespace DiscordBot
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using System.Configuration;
    using LorDeckImage;
    using DownloadMetadata;

    class Program
    {
        public static string? Token;
        public static string? Locale;

        private DiscordSocketClient? client;
        public static CommandService? Commands;
        public static IServiceProvider? Services;

        public static string TempDir => "temp";

        static void Main(string[] args)
        {
            Dictionary<string, string> SetConfig = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                SetConfig.Add(key, ConfigurationManager.AppSettings[key]);
            }

            Token = SetConfig["Token"];
            Locale = SetConfig["Locale"];

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            if (!Directory.Exists(TempDir))
            {
                new DirectoryInfo(TempDir).Create();
            }

            this.client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
            });
            this.client.Log += Log;
            Commands = new CommandService();
            Services = new ServiceCollection().BuildServiceProvider();
            this.client.MessageReceived += CommandRecieved;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            await this.client.LoginAsync(TokenType.Bot, Token);
            await this.client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null)
            {
                return;
            }

            // 発言者がBotの場合無視する
            if (message.Author.IsBot)
            {
                return;
            }

            var context = new CommandContext(this.client, message);

            // この機能を使って、発言の先頭が/ideck かどうかを判定する。
            string[] commandContext = message.Content.Split(" ");

            if (commandContext.Length < 2)
            {
                return;
            }

            if (commandContext[0] == "/ideck")
            {
                // デッキコードを取得→LoRDeckImageを使用してデッキ画像を作成する。
                // 作成した画像をチャットに投稿する。
                string deckcode = commandContext[1];
                Metadata metadata = new Metadata(Locale);
                Deck deck = new Deck(deckcode, metadata);

                if (deck != null)
                {
                    string savedImagePath = Path.Combine(TempDir, deckcode + ".png");
                    deck.SaveImageAsPng(savedImagePath);
                    await message.Channel.SendFileAsync(savedImagePath, deckcode);
                }
            }
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}