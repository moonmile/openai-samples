// Note: the Azure OpenAI client library for .NET is in preview.
// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5 

using Azure;
using Azure.AI.OpenAI;


string apikey = "e2e779b144e440a58d0a831f6821bb9d";
OpenAIClient client = new OpenAIClient(
    new Uri("https://sample-moonmile-openai.openai.azure.com/"),
    new AzureKeyCredential(apikey));


string prompt = 
"""
以下の内容を使って文章を作ってください。

主要なテーマは「光通信の安定的な接続について」です。
専門用語として、光通信の共有、NTT、ターミナルABCを使ってください。
追加文章として、「ターミナルABCは2025年のアップデートでより快適な回線速度を実現する予定です。」を入れてください。
""" ;

string prompt2 = 
"""
主要なテーマ: 光通信の安定的な接続について
専門用語: 光通信の共有、NTT、ターミナルABC
追加文書: ターミナルABCは2025年のアップデートでより快適な回線速度を実現する予定です。

この内容で文章を作ってください。
プログラムコードではなく文章です。
""" ;


// If streaming is selected
Response<Completions> response = await client.GetCompletionsAsync(
    new CompletionsOptions()
    {
        Prompts = { prompt2 },
        Temperature = (float)0.3,
        MaxTokens = 1000,
        NucleusSamplingFactor = (float)1,
        FrequencyPenalty = (float)0,
        PresencePenalty = (float)0,
        DeploymentName= "test-x",

    });
Completions completions = response.Value;
var text = completions.Choices.First().Text;

Console.WriteLine("出力：");
Console.WriteLine(text);

