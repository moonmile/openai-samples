
<template>
    <h1>AIスケジューラー</h1>
    <div>
        <div class="input-group">
            <input type="text" class="form-control me-2" v-model="input" />
            <button class="btn btn-primary" @click="clickHandler">送信</button>
        </div>
        <hr />
        <p v-html="output"></p>
    </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { 
    OpenAIClient, AzureKeyCredential, 
    type ChatRequestMessage } from '@azure/openai' 

const output = ref("初期メッセージ")
const input = ref("予定表を表示して。")
const messages = reactive([
    {"role": "system", "content": `
箇条書きで予定表を作ってください。

予定表のフォーマット：
- [日付] [内容]

現在の予定は以下の通りです。
[予定表はここから]
- 4/1 入社式
- 4/2 新人歓迎会
- 4/3 プログラム研修１
- 4/4 プログラム研修２
[予定表はここまで]

予定表を表示してください。
`},
])

const apikey = "e2e779b144e440a58d0a831f6821bb9d";
const url = "https://sample-moonmile-openai.openai.azure.com/";

const client = new OpenAIClient( url, new AzureKeyCredential( apikey ));

async function queryGPT4() {

    const response = await client.getChatCompletions( 
    "test-x", 
    messages as ChatRequestMessage[],
    {
        "maxTokens": 800,
        "temperature": 0.7,
        "topP": 0.95,
        "frequencyPenalty": 0,
        "presencePenalty": 0
    });
    return response?.choices[0]?.message?.content ?? "エラーが発生しました。";
}

async function clickHandler() {
    console.log("clickHandler " + input.value);
    messages.push({"role": "user", "content": input.value});
    var text = await queryGPT4();
    output.value = text.replace(/\n/g, "<br />");
    messages.push({"role": "assistant", "content": text});
}

</script>
