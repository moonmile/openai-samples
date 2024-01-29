using System.Text;
using System.Text.Json;

namespace SampleWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string apikey = "this-is-apikey";
        // private const string url = "https://sample-moonmile-openai-canada.azurewebsites.net/api/CallSchedule";
        private const string url = "http://localhost:7071/api/CallSchedule";


        /// <summary>
        /// チャットメッセージクラス
        /// </summary>
        public class ChatMessage
        {
            public string Role { get; set; } = "";
            public string Content { get; set; } = "";
        }
        /// <summary>
        /// チャットメッセージのリスト
        /// </summary>
        private List<ChatMessage> messages = new List<ChatMessage>();

        /// <summary>
        /// フォームがロードされたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            var text = """
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
                """;
            messages.Add(new ChatMessage() { Role = "system", Content = text });
            var json = JsonSerializer.Serialize(messages);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", apikey);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            textOutput.Text = result.Replace("\n", "\r\n");
            messages.Add(new ChatMessage() { Role = "assistant", Content = result });

        }

        /// <summary>
        /// Azure Functions にリクエストを送信する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonSend_Click(object sender, EventArgs e)
        {
            messages.Add(new ChatMessage() { Role = "user", Content = textInput.Text });
            var json = JsonSerializer.Serialize(messages);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", apikey);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            textOutput.Text = result.Replace("\n", "\r\n");
            messages.Add(new ChatMessage() { Role = "assistant", Content = result });


            textInput.Text = "予定表を表示して。";
            textOutput.Text = """
                現在の予定は以下の通りです。
                
                - 4/1 入社式
                - 4/2 新人歓迎会
                - 4/3 プログラム研修１
                - 4/4 プログラム研修２
                - 4/5 社内セキュリティ研修
                - 4/6 個人情報保護研修
                
                """;

        }
    }
}
