using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("文章をですます調に書き換えるプロンプト");

string prompt = """
つぎの文章を「ですます調」に書き換えてください。

このカレーのレシピでは、じゃがいもとにんじんとカレールーと豚肉を使用する。
まず、じゃがいもとにんじんは皮をむいて一口大に切る。
豚肉も同じように一口大に切る。
鍋にじゃがいもとにんじんと豚肉を入れ、水を加えて火にかける。
沸騰したら弱火にし、じっくり煮込む。
じゃがいもとにんじんが柔らかくなったら、カレールーを加える。
カレールーが溶けたら火を止め、器に盛り付けて完成となる。

文章：
""";

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatRequestUserMessage(prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);
