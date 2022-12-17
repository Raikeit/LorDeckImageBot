using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using LorDeckImage;

namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient? _client;
        public static CommandService? _commands;
        public static IServiceProvider? _services;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
            _client.MessageReceived += CommandRecieved;

            // TODO: Discord BOTのTokenを設定ファイルから読み込むようにする。
            // 設定ファイルはGitにアップロードしないこと
            string token = "hogehogehogehoge";
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// 何かしらのメッセージの受信
        /// </summary>
        /// <param name="msgParam"></param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            //デバッグ用メッセージを出力
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);
            //メッセージがnullの場合
            if (message == null)
                return;

            //発言者がBotの場合無視する
            if (message.Author.IsBot)
                return;


            var context = new CommandContext(_client, message);

            //ここから記述--------------------------------------------------------------------------
            var CommandContext = message.Content;

            // TODO: この機能を使って、発言の先頭が/ideck かどうかを判定する。
            // コマンド("おはよう")かどうか判定
            if (CommandContext == "おはよう")
            {
                await message.Channel.SendMessageAsync("Hello!");

                // TODO: デッキコードを取得→LoRDeckImageを使用してデッキ画像を作成する。
                // 作成した画像をチャットに投稿する。
            }


        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}