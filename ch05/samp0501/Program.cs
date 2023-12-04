using Azure;
using Azure.AI.OpenAI;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; // Azure OpenAIサービスのAPIキー
        string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; // Azure OpenAIサービスのエンドポイント

        string theme = "光通信の安定的な接続について";
        string terms = "光通信の共有、NTT、ターミナルABC";
        string additionalSentence = "ターミナルABCは2025年のアップデートでより快適な回線速度を実現する予定です。";

        string blogPost = await GenerateBlogPost(apiKey, endpoint, theme, terms, additionalSentence);
        Console.WriteLine(blogPost);
    }

    static async Task<string> GenerateBlogPost(string apiKey, string endpoint, string theme, string terms, string additionalSentence)
    {
        var credential = new AzureKeyCredential(apiKey);
        var client = new OpenAIClient(new Uri(endpoint), credential);

        string prompt = $"主要なテーマ：{theme}\n専門用語：{terms}\n追加文書：{additionalSentence}\n\nブログ記事：\n";

        var options = new CompletionsOptions
        {
            Prompts = { prompt },
            MaxTokens = 500,
            Temperature = (float)0.7,
        };

        Response<Completions> response = await client.GetCompletionsAsync(
            "test-x",
            options);
        string generatedText = response.Value.Choices[0].Text;

        string blogPost = generatedText.Trim();
        return blogPost;
    }
}
