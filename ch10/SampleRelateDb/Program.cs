using Azure.AI.OpenAI;
using Azure;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

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
あなたは書籍の在庫を管理するアシスタントです。
本の題名や著者名、出版社名を聞かれたら、現在の在庫を答えてください。
"""));


// 著者名や出版社名から本の情報を抜き出す
options.Tools.Add(
    new ChatCompletionsFunctionToolDefinition()
    {
        Name = "get_book",
        Description = "著者名や出版社名から本の情報を答える",
        Parameters = BinaryData.FromObjectAsJson(
            new {
                Type = "object",
                Properties = new
                {
                    author = new
                    {
                        Type = "string",
                        Description = "著者名"
                    },
                    publisher = new
                    {
                        Type = "string",
                        Description = "出版社名"
                    }
                },
                Reqired = new string[] { "text" }
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        ),
    });
// 本の題名から在庫数を返す
options.Tools.Add(
    new ChatCompletionsFunctionToolDefinition()
    {
        Name = "get_store",
        Description = "本の題名から在庫数を答える",
        Parameters = BinaryData.FromObjectAsJson(
            new {
                Type = "object",
                Properties = new
                {
                    title = new
                    {
                        Type = "string",
                        Description = "本の題名"
                    },
                },
                Reqired = new string[] { "title" }
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        ),
    });

// ユーザーの質問を作成
string prompt = "カレーの作り方を教えて。";
options.Messages.Add(new ChatRequestUserMessage(prompt));

// APIを呼び出す
var response = await client.GetChatCompletionsAsync(options);
var result = response.Value;
var reason = result.Choices.First().FinishReason;

if ( reason == CompletionsFinishReason.ToolCalls)
{
    var calls = result.Choices.First().Message.ToolCalls;
    var call = calls.FirstOrDefault() as ChatCompletionsFunctionToolCall;
switch (call?.Name ) {
    case "get_book":
        {
            dynamic obj = JObject.Parse(call.Arguments);
            string author = obj.author;
            string publisher = obj.publisher;

            Console.WriteLine($"function : {call.Name}");
            Console.WriteLine($"author : {author}");
            Console.WriteLine($"publisher : {publisher}");

            // 書籍データベースから検索する
            var db = new BooksContext();
            var query = db.Book;
            if (author != null ) { query.Where(x => x.Author.Contains(author)); }
            if ( publisher != null ) { query.Where(x => x.Publisher.Contains(publisher)); }
            var items = query.ToList();
            var next_prompt = "";
            if ( items.Count > 0 )
            {
                foreach (var it in items)
                {
                    next_prompt += $"- {it.Title} {it.Author} {it.Publisher} {it.Price}円 {it.Stock}冊\n";
                }
            } 
            else
            {
                next_prompt = "書籍が見つかりませんでした。";
            }
            options.Messages.Add(new ChatRequestAssistantMessage(next_prompt));
            options.Messages.Add(new ChatRequestUserMessage("要約してください。"));
        }
        break;
    case "get_store":
        {
            dynamic obj = JObject.Parse(call.Arguments);
            string title = obj.title;

            Console.WriteLine($"function : {call.Name}");
            Console.WriteLine($"title : {title}");

            // 書籍データベースから検索する
            var db = new BooksContext();
            var item = db.Book.Where(x => x.Title.Contains(title)).FirstOrDefault();
            var next_prompt = "";
            if ( item == null )
            {
                next_prompt = $"{title} という書籍は見つかりませんでした。";
            }
            else
            {
                next_prompt = $"{title} の在庫は {item.Stock} 冊です。";
            }
            options.Messages.Add(new ChatRequestAssistantMessage(next_prompt));
            options.Messages.Add(new ChatRequestUserMessage("要約してください。"));
        }
        break;
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


// 書籍データベースから検索する

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Publisher { get; set; } = "";
    public int Price { get; set; }
    public int Stock { get; set; }
}
class BooksContext : DbContext
{
    public DbSet<Book> Book { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=sampleai;Server=.;Encrypt=False;");
    }
}

