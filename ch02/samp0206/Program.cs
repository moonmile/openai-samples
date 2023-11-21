using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

// HTTPクライアントの生成
var client = new HttpClient();
// OpenAI APIキーの設定
string apiKey = 
  "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";
// リクエストヘッダーにAPIキーを追加
client.DefaultRequestHeaders.Add(
  "Authorization", $"Bearer {apiKey}");

// リクエストボディの作成
var requestBody = new
{
    model = "gpt-4",
    messages = new [] {
        new { role = "user" , content = "ここにりんごが10個あります。"},
        new { role = "assistant", content = "りんごが10個ある状況を描写します。…"},
        new { role = "user", content = "りんごを3個食べました。" },
        new { role = "assistant", content = "りんごを3個食べた後の状況を描写します。…"},
        new { role = "user", content = "残りのりんごはいくつでしょうか？"},
    },
    max_tokens = 256,
};

// リクエストをJSON形式に変換
string json = JsonSerializer.Serialize(requestBody);

var content = new StringContent(json, Encoding.UTF8, "application/json");
// OpenAI APIにリクエストを送信
HttpResponseMessage response = await client.PostAsync(
  "https://api.openai.com/v1/chat/completions", content);

// 応答を取得
string result = await response.Content.ReadAsStringAsync();
// 応答を表示(JSON形式)
Console.WriteLine(result);

