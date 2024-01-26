using Azure.AI.OpenAI;
using Azure;
using System.Text.Json;

namespace SampleScheduleMvc.Models
{
    public class PromptViewModel
    {
        private const string _apikey = "e2e779b144e440a58d0a831f6821bb9d";
        private const string _url = "https://sample-moonmile-openai.openai.azure.com/";
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



        private string _input = "";
        private string _output = "";

        public string Input
        {
            get { return _input; }
            set { _input = value; }
        }
        public string Output
        {
            get { return _output; }
            set { _output = value; }
        }

        private ChatCompletionsOptions _options;

        /// <summary>
        /// 最初のプロンプトを送信する
        /// </summary>
        public async Task SendInit()
        {
            _options = new ChatCompletionsOptions()
            {
                DeploymentName = "test-x",
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
            _options.Messages.Add(new ChatRequestSystemMessage("""
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

                """));

            var responseWithoutStream = await _client.GetChatCompletionsAsync(_options);
            var response = responseWithoutStream.Value;
            this.Output = response.Choices.First().Message.Content;
            _options.Messages.Add(new ChatRequestAssistantMessage(Output));
        }

        /// <summary>
        /// プロンプトを送信する
        /// </summary>
        public async Task Send()
        {
            _options.Messages.Add(new ChatRequestUserMessage(Input));
            var responseWithoutStream = await _client.GetChatCompletionsAsync(_options);
            var response = responseWithoutStream.Value;
            this.Output = response.Choices.First().Message.Content;
            _options.Messages.Add(new ChatRequestAssistantMessage(Output));
        }

        /// <summary>
        /// 最後の回答をストレージに保存
        /// </summary>
        public void Save()
        {
            // ここでは簡便のためメッセージとして表示させる
            var msg = (_options.Messages.Last() as ChatRequestAssistantMessage)?.Content;
        }

        /// <summary>
        /// シリアライズ
        /// </summary>  
        public string Serialize()
        {
            var messages = new List<ChatMessage>();
            foreach (var m in _options.Messages)
            {
                if (m is ChatRequestUserMessage)
                {
                    messages.Add(new ChatMessage()
                    {
                        Role = "user",
                        Content = (m as ChatRequestUserMessage).Content,
                    });
                }
                else if (m is ChatRequestAssistantMessage)
                {
                    messages.Add(new ChatMessage()
                    {
                        Role = "assistant",
                        Content = (m as ChatRequestAssistantMessage).Content,
                    });
                }
                else if (m is ChatRequestSystemMessage)
                {
                    messages.Add(new ChatMessage()
                    {
                        Role = "system",
                        Content = (m as ChatRequestSystemMessage).Content,
                    });
                }
            }
            var json =  JsonSerializer.Serialize(messages);
            return json;
        }
        /// <summary>
        /// デシリアライズ
        /// </summary>
        /// <param name="json"></param>
        public void Deserialize(string json)
        {
            var messages = JsonSerializer.Deserialize<List<ChatMessage>>(json);
            _options = new ChatCompletionsOptions()
            {
                DeploymentName = "test-x",
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
            foreach ( var it in messages)
            {
                if (it.Role == "user")
                {
                    _options.Messages.Add(new ChatRequestUserMessage(it.Content));
                }
                else if (it.Role == "assistant")
                {
                    _options.Messages.Add(new ChatRequestAssistantMessage(it.Content));
                }
                else if (it.Role == "system")
                {
                    _options.Messages.Add(new ChatRequestSystemMessage(it.Content));
                }
            }
        }

        public class ChatMessage
        {
            public string Role { get; set; } = "";
            public string Content { get; set; } = "";
        }
    }
}
