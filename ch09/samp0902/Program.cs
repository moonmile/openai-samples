using Azure.AI.OpenAI;
using Azure;
using Microsoft.EntityFrameworkCore;

string prompt = "マネジメント研修";
// 事前にデータから参照する
var db = new ScheduleContext();
var lst = db.schedule.Where(x => x.text.Contains(prompt)).ToList();
string message = "";

if (lst.Count == 0)
{
    message = "予定はありません";
} 
else
{
    foreach ( var item in lst )
    {
        message += $"{item.date} {item.text}\n";
    }   
}

// 回答文をOpenAI APIで作成する
const string apikey = "e2e779b144e440a58d0a831f6821bb9d";
const string url = "https://sample-moonmile-openai.openai.azure.com/";
OpenAIClient client = new OpenAIClient(
                new Uri(url),
                new AzureKeyCredential(apikey));

ChatCompletionsOptions options = new ()
{
    DeploymentName = "test-x",
    Temperature = (float)0.5,
    MaxTokens = 800,
    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
};
// システムメッセージを作成
options.Messages.Add(new ChatRequestSystemMessage("""
以下のメッセージをお客様に答えるように丁寧語にしてください。
"""));
// データをシステムメッセージで渡しておく
options.Messages.Add(new ChatRequestSystemMessage(message));

// APIを呼び出す
var response = await client.GetChatCompletionsAsync(options);
var result = response.Value;
var output = result.Choices.First().Message.Content;

Console.WriteLine($"質問: {prompt}");
Console.WriteLine($"回答: {output}");


class Schedule
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public string text { get; set; } = "";
}
class ScheduleContext : DbContext
{
    public DbSet<Schedule> schedule { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=sampleai;Server=.;Encrypt=False;");
    }
}

