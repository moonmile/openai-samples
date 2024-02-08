using Azure.AI.OpenAI;
using Azure;
using System.Text.Json;
using System.Security;
using Newtonsoft.Json.Linq;
using System.Text;

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
あなたは会議室の出席を取り仕切るアシスタントです。
指定された日に予約がはいっているかどうかを確認してください。
"""));

options.Messages.Add(new ChatRequestSystemMessage("""
会議室予約のフォーマットは次の通りです。


予約日付：[日付]
予約時刻：[開始時刻]から[終了時刻]まで
会議室：[会議室名]
予約者：[予約者名]

"""));

// 指定日で会議室を予約する
options.Tools.Add(
    new ChatCompletionsFunctionToolDefinition()
    {
        Name = "reserve_room",
        Description = "日付をして指定会議室の予約を行う。",
        Parameters = BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    date = new
                    {
                        Type = "string",
                        Format = "date-time",
                        Description = "日付"
                    },
                    start_time = new
                    {
                        Type = "string",
                        Description = "開始時刻"
                    },
                    end_time = new
                    {
                        Type = "string",
                        Description = "終了時刻"
                    },
                    room = new
                    {
                        Type = "string",
                        Description = "会議室名"
                    },
                    user = new
                    {
                        Type = "string",
                        Description = "予約者名"
                    },
                },
                Reqired = new string[] { "date", "start_time", "end_time", "room", "user" }

            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        ),
    });

// ユーザーの質問を作成
string prompt = """
カレーの作り方を教えてください。
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
    if (call?.Name == "reserve_room")
    {
        dynamic obj = JObject.Parse(call.Arguments);
        DateTime date = obj.date;
        string start_time = obj.start_time;
        string end_time = obj.end_time;
        string room = obj.room;
        string user = obj.user;


        // 会議予約のAPIを呼び出す
        ReserveRoom reserve = new ReserveRoom()
        {
            date = date,
            start_time = start_time,
            end_time = end_time,
            room = room,
            user = user
        };
        HttpClient cl = new HttpClient();
        var res = await cl.PostAsync("http://localhost:5249/reserve/add",
            new StringContent(
                JsonSerializer.Serialize(reserve),
                Encoding.UTF8, "application/json"));
        var res_body = await res.Content.ReadAsStringAsync();
        string output = res_body ?? "";
        Console.WriteLine($"質問: {prompt}");
        Console.WriteLine($"回答: {output}");
    }
}
else
{
    var output = "会議室の予約をお願いします。";
    Console.WriteLine("その他の応答");
    Console.WriteLine( output );
}

class ReserveRoom
{
    public DateTime date { get; set; }
    public string start_time { get; set; } = "";
    public string end_time { get; set; } = "";
    public string room { get; set; } = "";
    public string user { get; set; } = "";

}
