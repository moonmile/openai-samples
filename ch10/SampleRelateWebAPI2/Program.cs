using Azure.AI.OpenAI;
using Azure;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security;

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
あなたは領収書から経費登録を手伝うアシスタントです。
領収書の経費項目、日付、合計金額を伝えられたら、経費処理を行ってください。
項目が足りない場合は、領収書を見せてもらって、足りない情報を聞いてください。
"""));
options.Messages.Add(new ChatRequestSystemMessage("""
経費項目は次のフォーマットです。

- [日付] [経費項目] [金額]
"""));



// 著者名や出版社名から本の情報を抜き出す
options.Tools.Add(
    new ChatCompletionsFunctionToolDefinition()
    {
        Name = "get_expense_claim",
        Description = "経費項目、日付、金額を指定して経費精算を行う",
        Parameters = BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    Items = new
                    {
                        Type = "array",
                        Description = "経費項目のリスト",
                        Items = new
                        {
                            Type = "object",
                            Properties = new
                            {
                                kind = new
                                {
                                    Type = "string",
                                    Description = "経費項目"
                                },
                                date = new
                                {
                                    Type = "string",
                                    Format = "date-time",
                                    Description = "日付"
                                },
                                cost = new
                                {
                                    Type = "integer",
                                    Description = "金額"
                                },
                            },
                            Reqired = new string[] { "kind", "date", "cost" }
                        }
                    }
                }
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        ),
    });

// ユーザーの質問を作成
string prompt = """
以下の経費精算をお願いします。

- 2024/04/10 会議費       2000円
- 2024/04/20 新聞図書費    3000円
- 2024/04/30 交通費      13000円
""";
options.Messages.Add(new ChatRequestUserMessage(prompt));

// APIを呼び出す
var response = await client.GetChatCompletionsAsync(options);
var result = response.Value;
var reason = result.Choices.First().FinishReason;

if (reason == CompletionsFinishReason.ToolCalls)
{
    var calls = result.Choices.First().Message.ToolCalls;
    var call = calls.FirstOrDefault() as ChatCompletionsFunctionToolCall;
    if (call?.Name == "get_expense_claim")
    {
        var lst = JsonConvert.DeserializeObject<ClaimList>(call.Arguments);

        // 経費精算のWeb APIを呼び出す
        string next_prompt = "";
        if (lst?.Items.Count > 0)
        {
            next_prompt = $"以下の経費精算を行いました。\n\n";
            int sum = 0;
            foreach (var it in lst.Items)
            {
                string kind = it.Kind;
                DateTime date = it.Date;
                int cost = it.Cost;
                sum += cost;
                next_prompt += $"- {date} {kind} {cost}円\n";
            }
            next_prompt += $"\n合計 {sum}円\n";
        }
        else
        {
            next_prompt = "経費精算する項目がありませんでした。";
        }

        options.Messages.Add(new ChatRequestAssistantMessage(next_prompt));
        options.Messages.Add(new ChatRequestUserMessage("要約してください。"));
    }
    // APIを呼び出す
    var response2 = await client.GetChatCompletionsAsync(options);
    var result2 = response2.Value;
    var output2 = result2.Choices.First().Message.Content;

    Console.WriteLine($"質問: {prompt}");
    Console.WriteLine($"実際の回答: {output2}");
}
else
{
    var output = result.Choices.First().Message.Content;
    Console.WriteLine("その他の応答");
    Console.WriteLine($"output : {output}");
}

public class Claim
{
    public string Kind { get; set; } = "";
    public DateTime Date { get; set; }
    public int Cost { get; set; }
}
public class ClaimList
{
    public List<Claim> Items { get; set; } = new();
}

