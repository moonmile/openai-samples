using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

// 質問のプロンプト
string prompt = "猫とロボットと人工知能という用語を使って、オブジェクト指向を説明してください。\n";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);


Console.WriteLine("プレゼンスペナルティの実験コード");
double presence_penalty = -2.0 ;
Console.WriteLine($"FrequencyPenalty: {presence_penalty}");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    PresencePenalty = (float)presence_penalty,      // プレゼンスペナルティ
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

