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
        /// �`���b�g���b�Z�[�W�N���X
        /// </summary>
        public class ChatMessage
        {
            public string Role { get; set; } = "";
            public string Content { get; set; } = "";
        }
        /// <summary>
        /// �`���b�g���b�Z�[�W�̃��X�g
        /// </summary>
        private List<ChatMessage> messages = new List<ChatMessage>();

        /// <summary>
        /// �t�H�[�������[�h���ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            var text = """
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
        /// Azure Functions �Ƀ��N�G�X�g�𑗐M����
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


            textInput.Text = "�\��\��\�����āB";
            textOutput.Text = """
                ���݂̗\��͈ȉ��̒ʂ�ł��B
                
                - 4/1 ���Ў�
                - 4/2 �V�l���}��
                - 4/3 �v���O�������C�P
                - 4/4 �v���O�������C�Q
                - 4/5 �Г��Z�L�����e�B���C
                - 4/6 �l���ی쌤�C
                
                """;

        }
    }
}
