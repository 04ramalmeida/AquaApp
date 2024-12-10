using AquaApp.Models;
using AquaApp.Services;

namespace AquaApp.Pages;

public partial class AlertsPage : ContentPage
{
	private readonly ApiService _apiService;
    public AlertsPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var result = await _apiService.GetAlerts();
        if (result.ErrorMessage != null)
        {
            await DisplayAlert("Error", $"We were not able to obtain your alerts: {result.ErrorMessage}", "Cancel");
        }
        else
        {
            var alerts = result.Invoices;
            alerts.ForEach(alerts => alerts.Description = "A new invoice has been issued. To see the details, please check your invoices tab.");
            if (alerts.Count > 0)
            {
                AlertsList.ItemsSource = alerts;
                NoneLabel.IsVisible = false;
                AlertsList.IsVisible = true;
            }
        }
       
    }
}