using Azure;
using Azure.AI.OpenAI;
using System;
using System.Threading.Tasks;


string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; // Azure OpenAIサービスのAPIキー
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; // Azure OpenAIサービスのエンドポイント

string theme = "光通信の安定的な接続について";
string terms = "光通信の共有、NTT、ターミナルABC";
string additionalSentence = "ターミナルABCは2025年のアップデートでより快適な回線速度を実現する予定です。";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

string prompt = $"主要なテーマ：{theme}\n専門用語：{terms}\n追加文書：{additionalSentence}\n\nブログ記事：\n";

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 500,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string blogPost = res.Choices.First().Message.Content;
Console.WriteLine(blogPost);


