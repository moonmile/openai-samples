const { OpenAIClient, AzureKeyCredential } = require('@azure/openai');
// Azure OpenAIのエンドポイントとAPIキーを設定
const endpoint = "https://sample-moonmile-openai.openai.azure.com/";
const apiKey = process.env["AZURE_OPENAI_API_KEY"] ;
const client = new OpenAIClient( endpoint, new AzureKeyCredential(apiKey) );

async function main() {
  try {
    const messages = [
      { role: "system", content: "You are an AI assistant that helps people find information." },
      { role: "user", content: "Azure OpenAIについて詳しく説明してください。" },
    ]
    const result = await client.getChatCompletions("test-x", messages, { 
        temperature: 0.7,
        maxTokens: 800,
        topP: 0.95,
        frequencyPenalty: 0.0,
        presencePenalty: 0.0,
    });
    console.log(result.choices[0].message.content);
  } catch (error) {
    console.error("Error:", error);
  }
}

// チャット関数を実行
main();
