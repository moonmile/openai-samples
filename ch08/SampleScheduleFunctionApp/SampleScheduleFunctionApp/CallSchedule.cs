using System.Net;
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
            // _logger.LogInformation("C# HTTP trigger function processed a request.");
            // メッセージ配列を取得する
            var messages = await req.ReadFromJsonAsync<List<ChatMessage>>();
            if ( messages == null )
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }   
            // OpenAIのAPIを呼び出す
            var vm = new PromptViewModel();
            vm.SetMessage(messages);
            var output = await vm.Send();

            // レスポンスを返す
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(output);
            return response;
        }
    }

    /// <summary>
    /// チャットメッセージクラス
    /// </summary>
    public class ChatMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
