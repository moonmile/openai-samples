using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("箇条書きからレポートを作成するプロンプト");

string prompt = """
次の文章をレポート風に書き直してください。


テーマ：カレーの隠し味について

目的：
・一般的なカレーとはちょっと違った味にするレシピを作る。

手順：
・一般的なカレーの材料を示す（じゃがいもとにんじんとカレールーと豚肉）。
・隠し味を示す（りんごとバナナ）。
・材料を切る。
・材料を煮込む。
・カレールーを入れて煮込む。
・隠し味を入れる。

確認：
・隠し味を入れたときと、入れないときを比較する。
・味に差があることを確認する。
・被験者を募集する。

結論：
・隠し味がうまくいかないこともある。

レポート：
""";

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt ),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);
