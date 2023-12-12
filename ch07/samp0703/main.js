const { OpenAIClient, AzureKeyCredential } = require('@azure/openai');

// Azure OpenAIのエンドポイントとAPIキーを設定
const endpoint = "https://sample-moonmile-openai-jp.openai.azure.com/";
const apiKey = "a6eccd388fdf4358bb33b3b7568b487c";

const client = new OpenAIClient( endpoint, new AzureKeyCredential(apiKey) );

async function chatWithModel() {
    try {
        const messages = [
            { role: "user", content: "次の箇条書きを文章に直してください。" },
            { role: "user", content: "- 猫は可愛い。" },
            { role: "user", content: "- 猫は気まぐれである。" },
            { role: "user", content: "- ロボットは猫になることができるかもしれない。" },
        ]

        const result = await client.getChatCompletions("model-x", messages);
        console.log(result.choices[0].message.content);
    } catch (error) {
        console.error("Error:", error);
    }
}

// チャット関数を実行
chatWithModel();
