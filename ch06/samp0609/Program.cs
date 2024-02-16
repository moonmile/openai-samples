using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("ロールの実験コード");


var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatRequestSystemMessage("小学生に説明するように、やさしい言葉を使ってください。"),
      // new ChatRequestSystemMessage("大学生の説明するように、難しい言葉を使ってください。"),
      // new ChatRequestSystemMessage("裁判官の判決文のように、熟語を多く使ってください。"),
      new ChatRequestUserMessage ("- 猫は可愛い。"),
      new ChatRequestUserMessage ("- 猫は気まぐれである。"),
      new ChatRequestUserMessage("- ロボットは猫になることができる。"),
      new ChatRequestUserMessage ("これらの箇条書きを400文字位の文章にしてください。"),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

