using Azure;
using Azure.AI.OpenAI;


// エンドポイント
var url = "https://sample-moonmile-openai.openai.azure.com/";
// API-KEY は環境変数にしておく
string key = "e2e779b144e440a58d0a831f6821bb9d";


OpenAIClient client = new OpenAIClient(
  new Uri(url),
  new AzureKeyCredential(key));

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

これでブログ記事を書いてください。
""" ;


Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
  new ChatCompletionsOptions()
  {
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt2),      
    },
    Temperature = (float)0.7,
    MaxTokens = 800,


    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
    DeploymentName = "test-x",
  });


ChatCompletions response = responseWithoutStream.Value;
Console.WriteLine(response.Choices.First().Message.Content);

