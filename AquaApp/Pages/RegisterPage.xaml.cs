using AquaApp.Services;

namespace AquaApp.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService;
	public RegisterPage(ApiService apiService)
	{
		InitializeComponent();
        _apiService = apiService;
	}

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.Register(firstNameEntry.Text,
            lastNameEntry.Text,
            emailEntry.Text,
            addressEntry.Text,
            phoneNumberEntry.Text);

        if (!response.HasError)
        {
            await DisplayAlert("Registration", "Your account creation request was made, please await while it's reviewed.", "OK");
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }

    private void RegisterBtn_Clicked_1(object sender, EventArgs e)
    {

    }
}