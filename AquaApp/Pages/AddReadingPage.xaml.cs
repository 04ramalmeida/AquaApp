using AquaApp.Services;

namespace AquaApp.Pages;

public partial class AddReadingPage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly int _meterId;
    public AddReadingPage(ApiService apiService, int meterId)
    {
        InitializeComponent();
        _apiService = apiService;
        _meterId = meterId;
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(usageAmountEntry.Text))
        {
            await DisplayAlert("Error", "Please make sure the usage entry is not empty.", "Cancel");
            return;
        }

        var response = await _apiService.AddUserReading(_meterId, Convert.ToDouble(usageAmountEntry.Text));

        if (!response.HasError)
        {
            await DisplayAlert("Success", "Your reading was succesfully added!", "OK");
            await Navigation.PushAsync(new MetersPage(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }
}