# Get Started

## For Windows

### 1. .Net 6.0をインストール

下記のページから.Net 6.0をダウンロード&インストールします。

https://dotnet.microsoft.com/ja-jp/download

### 2. アプリケーションをダウンロードする

https://github.com/Raikeit/LorDeckImageBot/releases

上記Releaseページから最新バージョンの`LorDeckImageBot.zip`をダウンロードする。

### 3. ダウンロードしたzipファイルを解凍する

### 4. DiscordBotのトークンを発行する

DiscordBotの作成方法はこちら等を参考にしてください。

https://cod-sushi.com/discord-py-token/

使用するDiscordサーバーに作成したBotを招待しておきます。

### 5. 設定ファイルにトークンを入力する

`DiscordBot.dll.config`をテキストエディタで開き、`hogehogehoge`の部分に発行したトークンを入力してください。

```
<configuration>
	<appSettings>
		<add key="Token" value="hogehogehoge" />
		<add key="Locale" value="ja_jp" />
	</appSettings>
</configuration>
```

### 6. 必要ファイルのダウンロード

`DownloadMetadata.exe`を実行してください。
処理に数分かかります。
カードの追加等パッチがある度にこれを実行することで、データが最新版に更新されます。

### 7. Botの実行

`DiscordBot.exe`を実行してください。

### 8. Discordチャットでの使用方法

`/ideck CICQCAQDAMAQKBQBAIDAMFI6AMBAMFQ2HICQCAYCBQHSKKABAIBAMJRNAA`

上記のように`/ideck`の後ろに半角スペースとデッキコードをチャットで発言します。
Botから画像が返信されたら成功です。

![bot-chat-replay](https://pbs.twimg.com/media/FkmcW-PUEAAgAGF?format=png&name=small)