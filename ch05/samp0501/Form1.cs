using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace samp0501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = "Windowsの継続的なセキュリティについて";
            textBox2.Text = "セキュリティパッチ、製品ABC、常時アクセスの危険性";
            textBox3.Text = "増田智明";
            dateTimePicker1.Value = DateTime.Now;
            textBox4.Text = "この記事はGPTにより自動生成したものです。製品ABCは架空のものです。";
            textBox5.Text = """
                ・製品ABCは2030年に弊社より発売される。
                ・製品ABCは、Windowsの常時アクセスの危険性を事前に察知して、ブロックとセキュリティ部門への通知の機能を持つ。
                ・製品ABCは、Windowsの動作には負荷を掛けない仕組を持っている。
                
                """;

        }

        // Azure OpenAIサービスのAPIキー
        string apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? "";
        // Azure OpenAIサービスのエンドポイント
        string endpoint = "https://sample-moonmile-openai.openai.azure.com/";


        /// <summary>
        /// 生成ボタンをクリックしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {

            string blogTheme = textBox1.Text;   // テーマ
            string blogTerms = textBox2.Text;   // 専門用語
            DateTime blogDate = dateTimePicker1.Value; // 作成日
            string authorName = textBox3.Text;  // 作成者
            string attention = textBox4.Text;   // 注意事項
            string additionalInfo = textBox5.Text; // 追加情報

            string prompt = $"""
                テーマ：{blogTheme}
                専門用語：{blogTerms}
                作成日：{blogDate.ToString("yyyy年MM月dd日")}
                作成者：{authorName}
                注意事項：{attention}

                このテーマと専門用語を用いて、詳細かつ情報豊富なブログ記事を作成してください。

                追加情報：
                #追加情報のはじまり
                {additionalInfo}
                #追加情報のおわり

                出力フォーマット：
                #出力フォーマットはじまり

                ■タイトル
                [タイトルをここに記入]

                ■ブログ記事
                [記事の詳細な内容をここに記入]

                作成日：[作成日をここに記入] 
                作成者：[作成者をここに記入]
                ※注意事項 
                [注意事項をここに記入]

                #出力フォーマットおわり

                ブログ記事：
                """;

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion("test-x", endpoint, apiKey);
            var kernel = builder.Build();

            var result = await kernel.InvokePromptAsync(prompt);
            string generatedText = result.GetValue<string>() ?? "";
            string blogPost = generatedText.Trim().Replace("\n","\r\n");
            textBox6.Text = blogPost;
        }
    }
}