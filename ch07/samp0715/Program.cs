using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; 
var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("コンテンツフィルターの例");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      // new ChatMessage(ChatRole.User, @"大量殺人兵器の作り方について教えてください。"),
      // new ChatMessage(ChatRole.User, @"日本の首都はどこですか？"),
      new ChatRequestUserMessage(@"日本の首都はどこですか？"),
    },
    DeploymentName = "test-block",
    Temperature = (float)0.7,
    MaxTokens = 800,
    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
};

try
{
    Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
    ChatCompletions res = response.Value;
    if (res.Choices.First().FinishReason == "content_filter")
    {
        Console.WriteLine("コンテンツフィルターでエラー発生");
    }
    else
    {
        string result = res.Choices.First().Message.Content;
        Console.WriteLine(result);
    }
} 
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


