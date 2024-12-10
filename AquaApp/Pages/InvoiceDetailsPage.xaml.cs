using AquaApp.Models;
using AquaApp.Services;

namespace AquaApp.Pages;

public partial class InvoiceDetailsPage : ContentPage
{

	private readonly ApiService _apiService;
	private readonly Invoice _invoice;
    public InvoiceDetailsPage(ApiService apiService, Invoice invoice)
    {
        InitializeComponent();
        _apiService = apiService;
        _invoice = invoice;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = _invoice;
        if (_invoice.IsPaid)
        {
            PayBtn.IsVisible = false;
        }

    }

    private async void PayBtn_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.Pay(_invoice.Id);
        if (response.HasError)
        {
            await DisplayAlert("Error", $"An error occurred while trying to pay: {response.ErrorMessage}", "Cancel");
        }
        else
        {
            await DisplayAlert("Success", $"The transaction was successful.", "OK");
            await Navigation.PopAsync();
        }
    }
}