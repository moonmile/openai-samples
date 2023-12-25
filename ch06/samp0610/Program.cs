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
      new ChatMessage(ChatRole.System, "あなたは大学の先生です。"),
      new ChatMessage(ChatRole.System, "学生に講義するように話してください。"),
      new ChatMessage(ChatRole.User, "- 猫は可愛い。"),
      new ChatMessage(ChatRole.User, "- 猫は気まぐれである。"),
      new ChatMessage(ChatRole.User, "- ロボットは猫になることができる。"),
      new ChatMessage(ChatRole.User, "これらの箇条書きを400文字位の文章にしてください。"),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

