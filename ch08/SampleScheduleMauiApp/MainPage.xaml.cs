namespace SampleScheduleMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                _vm = new PromptViewModel();
                this.BindingContext = _vm;
                _vm.SendInit();
                /*
                _vm.Input = "予定表を表示して。";
                _vm.Output = """
                - 4/1 入社式
                - 4/2 新人歓迎会
                - 4/3 プログラム研修１
                - 4/4 プログラム研修２
                - 4/5 社内セキュリティ研修
                - 4/6 個人情報保護研修
                """;
                */
            };
        }

        PromptViewModel _vm;

        private async void clickSave(object sender, EventArgs e)
        {
            var msg = _vm.Output;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = System.IO.Path.Combine(path, "schedule.txt");
            System.IO.File.WriteAllText(filename, msg);
            await Application.Current.MainPage.DisplayAlert("保存", msg, "OK");
        }
    }

}
