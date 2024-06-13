using Azure.AI.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Security.Policy;

namespace SampleScheduleWpf
{
    public class PromptViewModel : ObservableObject
    {
        private const string _model = "test-x";
        private string _apikey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? "";
        private const string _url = "https://sample-moonmile-openai.openai.azure.com/";

        private Kernel _kernel;
        private IChatCompletionService _service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PromptViewModel()
        {
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                _model,
                _url,
                _apikey);
            _kernel = builder.Build();
            _service = _kernel.GetRequiredService<IChatCompletionService>();

            this.SendCommand = new RelayCommand(this.Send);
            this.SaveCommand = new RelayCommand(this.Save);
        }

        private string _input = "";
        private string _output = "";

        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value, nameof(Input)); }
        }
        public string Output
        {
            get { return _output; }
            set { SetProperty(ref _output, value, nameof(Output)); }
        }

        public RelayCommand SendCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        private ChatHistory _history = new ChatHistory();


        /// <summary>
        /// 最初のプロンプトを送信する
        /// </summary>
        public async void SendInit()
        {
            _history.AddSystemMessage(
                """
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

                """);

            var response = await _service.GetChatMessageContentAsync(
                                           _history,
                                           kernel: _kernel);
            // 応答を取得
            string combinedResponse = response.Items.OfType<TextContent>().FirstOrDefault()?.Text ?? "";
            this.Output = combinedResponse;
            // AIの応答を履歴に追加
            _history.AddAssistantMessage(combinedResponse);
        }

        /// <summary>
        /// プロンプトを送信する
        /// </summary>
        public async void Send()
        {
            _history.AddUserMessage(Input);
            var response = await _service.GetChatMessageContentAsync(
                                           _history,
                                           kernel: _kernel);
            // 応答を取得
            string combinedResponse = response.Items.OfType<TextContent>().FirstOrDefault()?.Text ?? "";
            this.Output = combinedResponse;
            // AIの応答を履歴に追加
            _history.AddAssistantMessage(this.Output);
        }

        /// <summary>
        /// 最後の回答をストレージに保存
        /// </summary>
        public void Save()
        {
            // ここでは簡便のためメッセージとして表示させる
            var msg = _history.Last()?.Content;
            // 保存ダイアログを開く
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "schedule"; 
            dlg.DefaultExt = ".txt"; 
            dlg.Filter = "Text documents (.txt)|*.txt"; 
            if ( dlg.ShowDialog() == true )
            {
                var filename = dlg.FileName;
                System.IO.File.WriteAllText(filename, msg);
                MessageBox.Show("保存しました。", "AIスケジューラー");
            }
        }
    }
}
