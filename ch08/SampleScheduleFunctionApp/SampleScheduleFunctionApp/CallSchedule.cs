using System.Net;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace SampleScheduleFunctionApp
{
    public class CallSchedule
    {
        private readonly ILogger _logger;

        public CallSchedule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CallSchedule>();
        }

        [Function("CallSchedule")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // ���b�Z�[�W�z����擾����
            var messages = await req.ReadFromJsonAsync<List<ChatMessage>>();
            if ( messages == null )
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }   
            // OpenAI��API���Ăяo��
            var vm = new PromptViewModel();
            vm.SetMessage(messages);
            var output = await vm.Send();

            // ���X�|���X��Ԃ�
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(output);
            return response;
        }
    }

    /// <summary>
    /// �`���b�g���b�Z�[�W�N���X
    /// </summary>
    public class ChatMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }


    public class PromptViewModel
    {
        private const string _apikey = "e448825271654bcf9bfa65e073636924";
        private const string _url = "https://sample-moonmile-openai-canada.openai.azure.com/";
        private OpenAIClient _client;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public PromptViewModel()
        {
            _client = new OpenAIClient(
                new Uri(_url),
                new AzureKeyCredential(_apikey));
        }

        private ChatCompletionsOptions _options;


        /// <summary>
        /// ChatMessage�z���ݒ肷��
        /// </summary>
        /// <param name="chatMessages"></param>
        public void SetMessage(List<ChatMessage> chatMessages )
        {
            // �I�v�V������ݒ肷��
            _options = new ChatCompletionsOptions()
            {
                DeploymentName = "test-x",
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
            // ���b�Z�[�W��ݒ肷��
            foreach ( var chat in chatMessages )
            {
                switch ( chat.Role )
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
        /// �v�����v�g�𑗐M����
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
