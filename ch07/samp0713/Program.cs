#if true

/**
 * Semantic Kernel を使った場合
 */

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
    "test-x",
    "https://sample-moonmile-openai.openai.azure.com/",
    Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? "");
var kernel = builder.Build();

Console.WriteLine("チャットの例");
// チャットの履歴をためておく
var history = new ChatHistory();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while ( true ) {
    Console.Write("You: ");
    var prompt = Console.In.ReadLine();
    if ( string.IsNullOrEmpty(prompt) )
    {
        break;
    }
    // ユーザーの入力を履歴に追加
    history.AddUserMessage(prompt);
    var response = await chatCompletionService.GetChatMessageContentAsync(
                                   history,
                                   kernel: kernel);
    // 応答を取得
    string combinedResponse = response.Items.OfType<TextContent>().FirstOrDefault()?.Text ?? "";
    Console.WriteLine("AI: " + combinedResponse);
    // AIの応答を履歴に追加
    history.AddAssistantMessage(combinedResponse);
}

#else


using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("チャットの例");
// チャットの履歴をためておく
var messages = new List<ChatRequestMessage>();

while( true ) {
    Console.Write("You: ");
    var prompt = Console.In.ReadLine();
    if ( string.IsNullOrEmpty(prompt) )
    {
        break;
    }
    messages.Add(new ChatRequestUserMessage(prompt));

    var options = new ChatCompletionsOptions("model-x", messages)
    {
        MaxTokens = 800,
        Temperature = (float)0.7,
    };
    Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
    ChatCompletions res = response.Value;
    string result = res.Choices.First().Message.Content;
    Console.WriteLine("AI: " + result);
    messages.Add(new ChatRequestAssistantMessage(result));
}

#endif
