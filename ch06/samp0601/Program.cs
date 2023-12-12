using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

// 質問のプロンプト
string prompt = "量子コンピュータについて詳しく教えてください。\n" ;

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);


Console.WriteLine("最大応答の実験コード");
var max_tokens = 800 ;
Console.WriteLine($"MaxTokens: {max_tokens}");

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatMessage(ChatRole.User, prompt),
      // プロンプトで指示を追加する
      new ChatMessage(ChatRole.User, "説明文を100文字程度にしてください。"),
    },
    DeploymentName = "model-x",
    MaxTokens = max_tokens,         // 最大応答の長さ
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);

