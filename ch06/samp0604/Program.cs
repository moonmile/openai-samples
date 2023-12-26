using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

// 質問のプロンプト
// string prompt = "宇宙について面白い事実を教えてください。\n" ;
string prompt = "猫とロボットと人工知能で、昔話を作ってください。\n";
var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);


Console.WriteLine("温度の実験コード");
double temperature = 0.8 ;
Console.WriteLine($"Temperature: {temperature}");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)temperature,      // 温度を指定する
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

