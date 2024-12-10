using AquaApp.Models;
using AquaApp.Services;

namespace AquaApp.Pages;

public partial class InvoicesPage : ContentPage
{
	private readonly ApiService _apiService;
    public InvoicesPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var Invoices = await _apiService.GetInvoices();
        if (Invoices.ErrorMessage != null)
        {
            await DisplayAlert("Error", $"An error occurred : {Invoices.ErrorMessage}", "Cancel");
        }
        else
        {
            InvoicesList.ItemsSource = Invoices.Invoices;
            NoneLabel.IsVisible = false;
            InvoicesList.IsVisible = true;
        }


    }

    private async void InvoicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tappedInvoice = e.CurrentSelection.FirstOrDefault() as Invoice;
        await Navigation.PushAsync(new InvoiceDetailsPage(_apiService, tappedInvoice));
    }
}