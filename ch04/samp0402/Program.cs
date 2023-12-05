using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; 

// ブログ記事のテーマ
string blogTheme = "最新のテクノロジートレンド";
// ブログ記事を生成するためのプロンプト
string prompt = $"次のテーマに基づいて詳細なブログ記事を書いてください: '{blogTheme}'\n";

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

var options = new CompletionsOptions
{
    Prompts = { prompt },
    DeploymentName = "model-x",
    MaxTokens = 500,
    Temperature = (float)0.7,
};




Response<Completions> response = await client.GetCompletionsAsync(options);
string generatedText = response.Value.Choices[0].Text;
string blogPost = generatedText.Trim();

Console.WriteLine(blogPost);


