﻿using Azure;
using Azure.AI.OpenAI;

// Azure OpenAIサービスのAPIキー
string apiKey = "a6eccd388fdf4358bb33b3b7568b487c"; 
// Azure OpenAIサービスのエンドポイント
string endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/"; 

var credential = new AzureKeyCredential(apiKey);
var client = new OpenAIClient(new Uri(endpoint), credential);

Console.WriteLine("チャットの例");
// チャットの履歴をためておく
var messages = new List<ChatRequestMessage>();

while( true ) {
    Console.Write("You: ");
    var prompt = Console.In.ReadLine();
    if ( string.IsNullOrEmpty(prompt) )
    {
        break;
    }
    messages.Add(new ChatRequestUserMessage(prompt));

    var options = new ChatCompletionsOptions("model-x", messages)
    {
        MaxTokens = 800,
        Temperature = (float)0.7,
    };
    Response<ChatCompletions> response = await client.GetChatCompletionsAsync(options);
    ChatCompletions res = response.Value;
    string result = res.Choices.First().Message.Content;
    Console.WriteLine("AI: " + result);
    messages.Add(new ChatRequestAssistantMessage(result));
}


