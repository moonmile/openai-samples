using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("箇条書きからビジネスメールを作成するプロンプト");

string prompt = """
つぎの文章をビジネスメールに書き換えてください。文字は400文字程度でお願いします。

問い合わせのカレーのレシピを伝える。
材料は、じゃがいもとにんじんとカレールーと豚肉。
隠し味に、りんごとバナナを入れる。

・じゃがいもとにんじんは、皮をむいて、一口大に切る。
・豚肉も同じように一口大に切る。
・沸騰したら弱火にし、じっくり煮込む。
・じゃがいもとにんじんが柔らかくなったら、カレールーを加える。
・すてきなデザインの皿でお楽しみください。

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
