﻿using Azure.AI.OpenAI;
using Azure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SampleScheduleWpf
{
    public class PromptViewModel : Prism.Mvvm.BindableBase
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

            this.SendCommand = new DelegateCommand(this.Send);
            this.SaveCommand = new DelegateCommand(this.Save);
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

        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        private ChatCompletionsOptions _options;

        /// <summary>
        /// 最初のプロンプトを送信する
        /// </summary>
        public async void SendInit()
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
        public async void Send()
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
