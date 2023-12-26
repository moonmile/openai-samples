using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("ブログを生成するプロンプト");
string prompt = """
テーマ：カレーの作り方について

このテーマを用いて、400文字程度のブログ記事を書いてください。
口調は小学生にでもわかるように、やさしい言葉を使ってください。

文章：
""";

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt ),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);
