// Note: The Azure OpenAI client library for .NET is in preview.
// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5
using Azure;
using Azure.AI.OpenAI;

OpenAIClient client = new OpenAIClient(
  new Uri("https://sample-moonmile-openai.openai.azure.com/"),
  new AzureKeyCredential(Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? ""));

  Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
  new ChatCompletionsOptions()
  {
      DeploymentName = "test-x",
    Messages =
    {
      new ChatRequestSystemMessage(@"You are an AI assistant that helps people find information."),
      new ChatRequestUserMessage(@"Azure OpenAIについて詳しく説明してください。"),      
      // new ChatMessage(ChatRole.Assistant, @"Azure OpenAIは、Microsoft Azure上で動作するAIプラットフォームです。OpenAIは、人工知能の研究・開発を行う非営利組織であり、その技術を活用したAIサービスを提供しています。Azure OpenAIは、OpenAIの技術を利用して、自然言語処理、画像認識、音声認識などのタスクを実行するためのAPIを提供しています。これにより、開発者は簡単にAIを組み込んだアプリケーションを開発することができます。Azure OpenAIは、高度なAI技術を利用したアプリケーションの開発に適しており、企業や開発者にとって非常に有用なプラットフォームです。"),
    },
    Temperature = (float)0.5,
    MaxTokens = 800,
    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
  });

ChatCompletions response = responseWithoutStream.Value;

Console.WriteLine(response.Choices.First().Message.Content);

