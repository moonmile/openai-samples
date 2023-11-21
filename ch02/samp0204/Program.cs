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
// 要約する文章
string task   = "C#で簡単なHello World関数を作成してください。";
// Codex APIを使用してコードを生成するためのプロンプト
string prompt = $"// {task}\n";

// リクエストボディの作成
var requestBody = new
{
    model = "gpt-3.5-turbo-instruct",
    prompt = prompt,
    temperature = 1,
    max_tokens = 256,
    top_p = 1,
    frequency_penalty = 0,
    presence_penalty = 0
};

// リクエストをJSON形式に変換
string json = JsonSerializer.Serialize(requestBody);

var content = new StringContent(json, Encoding.UTF8, "application/json");
// OpenAI APIにリクエストを送信
HttpResponseMessage response = await client.PostAsync(
  "https://api.openai.com/v1/completions", content);

// 応答を取得
string result = await response.Content.ReadAsStringAsync();
// 応答を表示(JSON形式)
Console.WriteLine(result);

