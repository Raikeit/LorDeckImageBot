# Get Started

## For Windows

### アプリケーションをダウンロードする

https://github.com/Raikeit/LorDeckImageBot/releases

上記Releaseページから最新バージョンのLorDeckImageBot.zipをダウンロードする。

### ダウンロードしたzipファイルを解凍する

### DiscordBotのトークンを発行する

DiscordBotの作成方法はこちら等を参考にしてください。

https://cod-sushi.com/discord-py-token/

使用するDiscordサーバーに作成したBotを招待しておきます。

### 設定ファイルにトークンを入力する

`DiscordBot.dll.config`をテキストエディタで開き、`hogehogehoge`の部分に発行したトークンを入力してください。

```
<configuration>
	<appSettings>
		<add key="Token" value="hogehogehoge" />
		<add key="Locale" value="ja_jp" />
	</appSettings>
</configuration>
```

### 必要ファイルのダウンロード

`DownloadMetadata.exe`を実行してください。
処理に数分かかります。
カードの追加等パッチがある度にこれを実行することで、データが最新版に更新されます。

### Botの実行

`LorDeckImage.exe`を実行してください。