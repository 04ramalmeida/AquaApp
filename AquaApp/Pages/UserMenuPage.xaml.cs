using AquaApp.Services;

namespace AquaApp.Pages;

public partial class UserMenuPage : ContentPage
{
	private readonly ApiService _apiService;
    public UserMenuPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var profilePicUrl = await _apiService.GetProfilePic();
        if (profilePicUrl.ErrorMessage != null)
        {
            await DisplayAlert("Error", "Error getting user profile picture", "Cancel");
        }
        else
        {
            ProfilePic.Source = profilePicUrl.Item1.Url;
        }
        

    }

    private async void MyAccount_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyAccount(_apiService));
    }

    private async void logOut_Tapped(object sender, TappedEventArgs e)
    {
        var result = await _apiService.Logout();
        if (result.ErrorMessage != null)
        {
            await DisplayAlert("Error", "Logout unsuccesfull", "Cancel");
        }
        else
        {
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
    }
}