#Note: The openai-python library support for Azure OpenAI is in preview.
#Note: This code sample requires OpenAI Python library version 0.28.1 or lower.

import os
import openai

openai.api_type = "azure"
openai.api_base = "https://sample-moonmile-openai.openai.azure.com/"
openai.api_version = "2023-07-01-preview"
openai.api_key = os.getenv("OPENAI_API_KEY")

message_text = [
  {"role":"system","content":"You are an AI assistant that helps people find information."},
  {"role":"user","content":"Azure OpenAIについて詳しく説明してください。"},
]

completion = openai.ChatCompletion.create(
  engine="test-x",
  messages = message_text,
  temperature=0.7,
  max_tokens=800,
  top_p=0.95,
  frequency_penalty=0,
  presence_penalty=0,
  stop=None
)


# 応答を表示します。
print(completion.choices[0].message['content'])
