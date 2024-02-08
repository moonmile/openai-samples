using Azure.AI.OpenAI;
using Azure;

// システムメッセージにデータを埋め込む
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
以下のデータを使って、予定から日付を答えてください。
データの形式は「日付:予定」です。
予定が見つからない場合は「予定はありません」と答えてください。
"""));

// データをシステムメッセージで渡しておく
options.Messages.Add(new ChatRequestSystemMessage("""
4/1:入社式
4/2:新人歓迎会
4/3:プログラム研修１
4/4:プログラム研修２
4/5:セキュリティ研修
4/6:ネットワーク研修
"""));

// ユーザーの質問を作成
string prompt = "セキュリティ研修はいつですか？";
options.Messages.Add(new ChatRequestUserMessage( prompt ));
// APIを呼び出す
var response = await client.GetChatCompletionsAsync(options);
var result = response.Value;
var output = result.Choices.First().Message.Content;

Console.WriteLine($"質問: {prompt}");
Console.WriteLine($"回答: {output}");

