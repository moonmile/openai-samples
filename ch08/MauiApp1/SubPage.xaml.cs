using System.Diagnostics;

namespace MauiApp1;

public partial class SubPage : ContentPage
{
	public SubPage()
	{
		InitializeComponent();
	}

    private async void OnClicked(object sender, EventArgs e)
    {
		/*
		var navi = Application.Current.MainPage as NavigationPage;
		var page = navi.CurrentPage;

		page = new MainPage();
		Debug.WriteLine("DisplayAlert start");
		await page.DisplayAlert("Alert", "You have been alerted", "OK");
		Debug.WriteLine("DisplayAlert end");
		*/



        Application.Current.MainPage.DisplayAlert("Alert", "You have been alerted", "OK");
        // DisplayAlert("Alert", "You have been alerted", "OK");
    }
}