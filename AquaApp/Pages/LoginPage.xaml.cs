using AquaApp.Services;

namespace AquaApp.Pages;

public partial class LoginPage : ContentPage
{
	private readonly ApiService _apiService;
    public LoginPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void LoginBtn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordentry.Text))
        {
            await DisplayAlert("Error", "Please make sure none of the text entries are empty.", "Cancel");
            return;
        }

        var response = await _apiService.Login(emailEntry.Text, passwordentry.Text);

        if (!response.HasError)
        {
           await DisplayAlert("Success", "Success", "Sucess");
            await Navigation.PushAsync(new MyAccount(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }

    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_apiService));
    }

    private async void TapRecover_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RecoveryPasswordPage(_apiService));
    }
}