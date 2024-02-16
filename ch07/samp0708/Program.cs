using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 


var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("文章を箇条書きにするプロンプト");

string prompt = """
次の文章を要約して3つの箇条書きにしてください。

ある日、豚肉と玉ねぎとじゃがいもは、おいしいカレーを作ることにしました。
豚肉は元気いっぱいで、「わたしはカレーにピリッとした味を出すのが得意だよ！」と自慢しました。
玉ねぎはやさしく微笑みながら、「わたしはカレーに甘みをプラスするのが得意だよ！」と言いました。
じゃがいもはおおらかに、「わたしはカレーにとろりとした食感を出すのが得意だよ！」と言いました。
そんな３人は、ハンドミキサーを使って材料を混ぜ合わせることにしました。
豚肉がピリッと、玉ねぎが甘みを、じゃがいもがとろりと、それぞれの得意な味を出すために、みんな力を合わせました。
そして、カレーの香りがキッチンに広がりました。みんなのお腹はグーグー鳴って、待ちきれません。
最後に、豚肉と玉ねぎとじゃがいもは、お皿に盛り付けました。
お箸を持って一口食べると、口の中に広がるおいしい味に、３人は大満足。
「やったね！おいしいカレーができたよ！」と、３人は喜びました。
その日の夕飯は、家族みんなで美味しいカレーを楽しみました。
豚肉も玉ねぎもじゃがいもも、みんなのお腹を満たすおいしいカレーを作ることができて、とっても嬉しかったのです。

箇条書き：
""";

var options = new ChatCompletionsOptions
{
    Messages =
    {
      new ChatRequestUserMessage(prompt),
    },
    DeploymentName = "model-x",
    MaxTokens = 800,
    Temperature = (float)0.7,
};

Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
ChatCompletions res = response.Value;
string result = res.Choices.First().Message.Content;
Console.WriteLine(result);
