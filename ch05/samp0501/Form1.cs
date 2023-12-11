using Azure.AI.OpenAI;
using Azure;

namespace samp0501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = "Windows�̌p���I�ȃZ�L�����e�B�ɂ���";
            textBox2.Text = "�Z�L�����e�B�p�b�`�A���iABC�A�펞�A�N�Z�X�̊댯��";
            textBox3.Text = "���c�q��";
            dateTimePicker1.Value = DateTime.Now;
            textBox4.Text = "���̋L����GPT�ɂ�莩�������������̂ł��B���iABC�͉ˋ�̂��̂ł��B";
            textBox5.Text = """
                �E���iABC��2030�N�ɕ��Ђ�蔭�������B
                �E���iABC�́AWindows�̏펞�A�N�Z�X�̊댯�������O�Ɏ@�m���āA�u���b�N�ƃZ�L�����e�B����ւ̒ʒm�̋@�\�����B
                �E���iABC�́AWindows�̓���ɂ͕��ׂ��|���Ȃ��d�g�������Ă���B
                
                """;

        }

        // Azure OpenAI�T�[�r�X��API�L�[
        string apiKey = "e2e779b144e440a58d0a831f6821bb9d";
        // Azure OpenAI�T�[�r�X�̃G���h�|�C���g
        string endpoint = "https://sample-moonmile-openai.openai.azure.com/";


        /// <summary>
        /// �����{�^�����N���b�N�����Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {

            string blogTheme = textBox1.Text;   // �e�[�}
            string blogTerms = textBox2.Text;   // ���p��
            DateTime blogDate = dateTimePicker1.Value; // �쐬��
            string authorName = textBox3.Text;  // �쐬��
            string attention = textBox4.Text;   // ���ӎ���
            string additionalInfo = textBox5.Text; // �ǉ����

            string prompt = $"""
                �e�[�}�F{blogTheme}
                ���p��F{blogTerms}
                �쐬���F{blogDate.ToString("yyyy�NMM��dd��")}
                �쐬�ҁF{authorName}
                ���ӎ����F{attention}

                ���̃e�[�}�Ɛ��p���p���āA�ڍׂ����L�x�ȃu���O�L�����쐬���Ă��������B

                �ǉ����F
                #�ǉ����̂͂��܂�
                {additionalInfo}
                #�ǉ����̂����

                �o�̓t�H�[�}�b�g�F
                #�o�̓t�H�[�}�b�g�͂��܂�

                ���^�C�g��
                [�^�C�g���������ɋL��]

                ���u���O�L��
                [�L���̏ڍׂȓ��e�������ɋL��]

                �쐬���F[�쐬���������ɋL��] 
                �쐬�ҁF[�쐬�҂������ɋL��]
                �����ӎ��� 
                [���ӎ����������ɋL��]

                #�o�̓t�H�[�}�b�g�����

                �u���O�L���F
                """;


            var credential = new AzureKeyCredential(apiKey);
            var client = new OpenAIClient(new Uri(endpoint), credential);
            var options = new CompletionsOptions
            {
                Prompts = { prompt },
                DeploymentName = "model-x",
                MaxTokens = 4000,
                Temperature = (float)0.7,
            };
            Response<Completions> response = await client.GetCompletionsAsync(options);
            string generatedText = response.Value.Choices[0].Text;
            string blogPost = generatedText.Trim();
            textBox6.Text = blogPost;

        }
    }
}