using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "e2e779b144e440a58d0a831f6821bb9d"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai.openai.azure.com/"; 
var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("画像生成の例");
Response<ImageGenerations> imageGenerations  = 
  await client.GetImageGenerationsAsync(
    new ImageGenerationOptions()
	{
      Prompt = "Azure OpenAIの漫画を表示してください。",
      Size = ImageSize.Size512x512,
    });
Uri imageUri = imageGenerations.Value.Data[0].Url;    
Console.WriteLine(imageUri);
