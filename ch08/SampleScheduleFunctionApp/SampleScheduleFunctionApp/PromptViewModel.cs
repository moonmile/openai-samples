﻿using Azure.AI.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleScheduleFunctionApp
{
    public class PromptViewModel
    {
        private const string _apikey = "e448825271654bcf9bfa65e073636924";
        private const string _url = "https://sample-moonmile-openai-canada.openai.azure.com/";
        private OpenAIClient _client;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PromptViewModel()
        {
            _client = new OpenAIClient(
                new Uri(_url),
                new AzureKeyCredential(_apikey));
        }

        private ChatCompletionsOptions _options;


        /// <summary>
        /// ChatMessage配列を設定する
        /// </summary>
        /// <param name="chatMessages"></param>
        public void SetMessage(List<ChatMessage> chatMessages)
        {
            // オプションを設定する
            _options = new ChatCompletionsOptions()
            {
                DeploymentName = "test-x",
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
            // メッセージを設定する
            foreach (var chat in chatMessages)
            {
                switch (chat.Role)
                {
                    case "user":
                        _options.Messages.Add(new ChatRequestUserMessage(chat.Content));
                        break;
                    case "assistant":
                        _options.Messages.Add(new ChatRequestAssistantMessage(chat.Content));
                        break;
                    case "system":
                        _options.Messages.Add(new ChatRequestSystemMessage(chat.Content));
                        break;
                }
            }
        }

        /// <summary>
        /// プロンプトを送信する
        /// </summary>
        public async Task<string> Send()
        {
            var responseWithoutStream = await _client.GetChatCompletionsAsync(_options);
            var response = responseWithoutStream.Value;
            var output = response.Choices.First().Message.Content;
            return output;
        }
    }
}
