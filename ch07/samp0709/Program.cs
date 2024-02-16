using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("箇条書きから文章を生成するプロンプト");

string prompt = """
つぎの箇条書きをもとに800文字ぐらいの文章を書いてください。

・カレーのレシピ
・材料は、じゃがいもとにんじんとカレールーと豚肉
・じゃがいもとにんじんは、皮をむいて、一口大に切る
・豚肉は、一口大に切る
・鍋に、じゃがいもとにんじんと豚肉を入れる
・水を入れて、火にかける
・沸騰したら、弱火にして、じっくり煮込む
・じゃがいもとにんじんが柔らかくなったら、カレールーを入れる
・カレールーが溶けたら、火を止める
・器に盛り付けて、完成

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
