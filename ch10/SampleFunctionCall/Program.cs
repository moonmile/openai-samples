using Azure.AI.OpenAI;
using Azure;
using System.Text.Json;

// システムメッセージにデータを埋め込む
const string apikey = "e2e779b144e440a58d0a831f6821bb9d";
const string url = "https://sample-moonmile-openai.openai.azure.com/";
OpenAIClient client = new OpenAIClient(
                new Uri(url),
                new AzureKeyCredential(apikey));

ChatCompletionsOptions options = new()
{
    DeploymentName = "test-x",
    Temperature = (float)0.5,
    MaxTokens = 800,
    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
};

// 役割を示しておく
options.Messages.Add(new ChatRequestSystemMessage("""
あなたは天気予報を答えるAIアシスタントです。
都道府県名から明日の天気を答えてください。
"""));

options.Tools.Add(
    new ChatCompletionsFunctionToolDefinition()
    {
        Name = "get_weather",
        Description = "都道府県を指定して天気を答える。",
        Parameters = BinaryData.FromObjectAsJson(
        new
        {
            Type = "object",
            Properties = new
            {
                location = new
                {
                    Type = "string",
                    Description = "都道府県名"
                }
            },
            Reqired = new string[] { "location" }
        },
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }
        ),
    });


// ユーザーの質問を作成
string prompt = "カレーの作り方を教えてください。";
options.Messages.Add(new ChatRequestUserMessage(prompt));
// APIを呼び出す
var response = await client.GetChatCompletionsAsync(options);
var result = response.Value;
var reason = result.Choices.First().FinishReason;

if (reason == CompletionsFinishReason.ToolCalls)
{
    Console.WriteLine("FunctionCall");
    var calls = result.Choices.First().Message.ToolCalls;
    var call = calls.FirstOrDefault() as ChatCompletionsFunctionToolCall;
    if (call?.Name == "get_weather")
    {
        var param = JsonSerializer.Deserialize<Param>(call.Arguments);
        Console.WriteLine($"場所: {param?.location}");

        // 実際は天気用のAPIを呼び出す
        // 仮にダミーの天気用法を返しておく
        string prompt2 = "明日の東京の天気は快晴です。";

        options.Messages.Clear();
        options.Messages.Add(new ChatRequestAssistantMessage(prompt2));
        options.Messages.Add(new ChatRequestUserMessage("要約してください。"));

    }
    // APIを呼び出す
    response = await client.GetChatCompletionsAsync(options);
    result = response.Value;
    var output = result.Choices.First().Message.Content;
    Console.WriteLine($"質問: {prompt}");
    Console.WriteLine($"回答: {output}");
} 
else 
{ 
    var output = result.Choices.First().Message.Content;
    Console.WriteLine($"質問: {prompt}");
    Console.WriteLine($"回答: {output}");
}

class Param
{
    public string location { get; set; } = "";
}


