using Azure;
using Azure.AI.OpenAI;

OpenAIClient client = new OpenAIClient(
  new Uri("https://sample-moonmile-openai.openai.azure.com/"),
  new AzureKeyCredential(Environment.GetEnvironmentVariable(
    "AZURE_OPENAI_API_KEY") ?? ""));

Response<Completions> completionsResponse = 
  await client.GetCompletionsAsync(
    new CompletionsOptions()
	{
      DeploymentName = "test-x",
      Prompts = { "Azure OpenAIについて400文字位で説明してください。\n\n文章：\n" },
      Temperature = (float)1,
      MaxTokens = 800,
      NucleusSamplingFactor = (float)0.5,
      FrequencyPenalty = (float)0,
      PresencePenalty = (float)0,
    });
Completions completions = completionsResponse.Value;
Console.WriteLine(completions.Choices.First().Text);
