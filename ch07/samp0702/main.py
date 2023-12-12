#Note: The openai-python library support for Azure OpenAI is in preview.
#Note: This code sample requires OpenAI Python library version 0.28.1 or lower.

import os
import openai

openai.api_type = "azure"
openai.api_base = "https://sample-moonmile-openai-jp.openai.azure.com/"
openai.api_version = "2023-07-01-preview"
openai.api_key = "a6eccd388fdf4358bb33b3b7568b487c"

message_text = [
    {"role":"user","content":"次の箇条書きを文章に直してください。"},
    {"role":"user","content":"- 猫は可愛い。"},
    {"role":"user","content":"- 猫は気まぐれである。"},
    {"role":"user","content":"- ロボットは猫になることができる。"},
]

response = openai.ChatCompletion.create(
  engine="model-x",
  messages = message_text,
  temperature=0.7,
  max_tokens=800,
  top_p=0.95,
  frequency_penalty=0,
  presence_penalty=0,
  stop=None
)

# 応答を表示します。
print(response.choices[0].message['content'])
