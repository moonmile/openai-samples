using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

// 質問のプロンプト
string prompt = "猫とロボットと人工知能で、昔話を作ってください。\n";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);


Console.WriteLine("頻度のペナルティの実験コード");
double frequency_penalty = 0.0 ;
Console.WriteLine($"FrequencyPenalty: {frequency_penalty}");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    FrequencyPenalty = (float)frequency_penalty,      // 頻度のペナルティ
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

