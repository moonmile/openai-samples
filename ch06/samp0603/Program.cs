using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; 

// 記事を生成するためのプロンプト
string prompt = """
量子コンピュータについて詳しく教えてください。

""";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);
int maxTokens = 4000 ;

Console.WriteLine("最大値の実験コード");

var options = new CompletionsOptions
{
    Prompts = { prompt },
    DeploymentName = "model-x",
    MaxTokens = maxTokens,
    Temperature = (float)0.7,
};


var response = await client.GetCompletionsStreamingAsync(options);

await foreach ( var it in response.EnumerateValues())
{
    if ( it.Choices.Count > 0 )
    {
        Console.Write(it.Choices[0].Text);
    }
}


