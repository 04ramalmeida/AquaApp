using AquaApp.Services;

namespace AquaApp.Pages;

public partial class RecoveryPasswordPage : ContentPage
{
    private readonly ApiService _apiService;
    public RecoveryPasswordPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.RecoveryPassword(emailEntry.Text);

        if (!response.HasError)
        {
            await DisplayAlert("Password recovery", "Check your email for the recovery token.", "OK");
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }

    private async void TapReset_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordPage(_apiService));
    }
}