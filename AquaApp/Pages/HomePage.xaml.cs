using AquaApp.Services;

namespace AquaApp.Pages;

public partial class HomePage : ContentPage
{
	private readonly ApiService _apiService;
    public HomePage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }


    private async void RequestBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_apiService));
    }
}