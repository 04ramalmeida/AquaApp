using AquaApp.Services;

namespace AquaApp.Pages;

public partial class ResetPasswordPage : ContentPage
{
    private readonly ApiService _apiService;
    public ResetPasswordPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.ResetPassword(emailEntry.Text,
            tokenEntry.Text, passwordEntry.Text);

        if (!response.HasError)
        {
            await DisplayAlert("Password reset", "Your password has been succesfully reset", "OK");
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }

    private  async void TapRecovery_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RecoveryPasswordPage(_apiService));
    }
}