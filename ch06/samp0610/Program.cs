using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("例文の実験コード");
var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.System, "小学生に説明するように、やさしい言葉を使ってください。"),
      new ChatMessage(ChatRole.User, "猫はかわいいですか？"),
      new ChatMessage(ChatRole.Assistant, "猫はかわいいね!!!"),
      new ChatMessage(ChatRole.User, "猫はきまぐれですか。"),
      new ChatMessage(ChatRole.Assistant, "そうですね。猫はきまぐれなときがあるよ!!!"),
      new ChatMessage(ChatRole.User, "ロボットは猫になることはできますか？"),
      new ChatMessage(ChatRole.Assistant, "そうですね!!! 現実の世界ではロボットは猫になりませんが、漫画の世界ではあるよ!!!"),
      new ChatMessage(ChatRole.User, "漫画にある猫ロボットは何ですか？"),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

