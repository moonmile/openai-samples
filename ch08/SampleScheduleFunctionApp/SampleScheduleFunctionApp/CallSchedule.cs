using System.Net;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var vm = new ViewModel();
            string output = await vm.SendInit();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString( output );

            return response;
        }
    }


    public class ViewModel 
    {
        private const string _apikey = "e448825271654bcf9bfa65e073636924";
        private const string _url = "https://sample-moonmile-openai-canada.openai.azure.com/";
        private OpenAIClient _client;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public ViewModel()
        {
            _client = new OpenAIClient(
                new Uri(_url),
                new AzureKeyCredential(_apikey));
        }

        public string Input { get; set; }
        public string Output { get; set; }

        private ChatCompletionsOptions _options;
        private List<ChatRequestMessage> _messages = new List<ChatRequestMessage>();


        /// <summary>
        /// �ŏ��̃v�����v�g�𑗐M����
        /// </summary>
        public async Task<string> SendInit()
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
                �ӏ������ŗ\��\������Ă��������B

                �\��\�̃t�H�[�}�b�g�F
                - [���t] [���e]

                ���݂̗\��͈ȉ��̒ʂ�ł��B
                [�\��\�͂�������]
                - 4/1 ���Ў�
                - 4/2 �V�l���}��
                - 4/3 �v���O�������C�P
                - 4/4 �v���O�������C�Q
                [�\��\�͂����܂�]

                �\��\��\�����Ă��������B

                """));

            var responseWithoutStream = await _client.GetChatCompletionsAsync(_options);
            var response = responseWithoutStream.Value;
            this.Output = response.Choices.First().Message.Content;
            _options.Messages.Add(new ChatRequestAssistantMessage(Output));
            return this.Output;
        }

        /// <summary>
        /// �v�����v�g�𑗐M����
        /// </summary>
        public async Task<string> Send()
        {
            _options.Messages.Add(new ChatRequestUserMessage(Input));
            var responseWithoutStream = await _client.GetChatCompletionsAsync(_options);
            var response = responseWithoutStream.Value;
            this.Output = response.Choices.First().Message.Content;
            _options.Messages.Add(new ChatRequestAssistantMessage(Output));
            return this.Output;
        }
    }
}
