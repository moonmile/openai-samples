using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

// 質問のプロンプト
string prompt = """
    次の文章の続きを、想像で作ってください。

    深海の探索中、突然珍しい生物に遭遇した。その生物は・・・
""";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);


Console.WriteLine("上位Pの実験コード");
double top_p = 0.8 ;
Console.WriteLine($"Top P: {top_p}");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    NucleusSamplingFactor = (float)top_p,      // 上位Pを指定する
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

