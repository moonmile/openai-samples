using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

string prompt = """
次の箇条書きを200文字程度の文章に書き換えてください。

- 猫は可愛い。
- 猫は気まぐれである。
- ロボットは猫になることができる。

文章：
""";

Console.WriteLine("コンソールで実行（入力候補形式）");


var options = new CompletionsOptions
{
    Prompts = { prompt },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};


Response<Completions> response = await client.GetCompletionsAsync(options);
string result = response.Value.Choices[0].Text;
Console.WriteLine(result);


