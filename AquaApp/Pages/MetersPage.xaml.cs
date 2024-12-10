using AquaApp.Models;
using AquaApp.Services;
using System.Collections.ObjectModel;

namespace AquaApp.Pages;

public partial class MetersPage : ContentPage
{
	private readonly ApiService _apiService;
    private List<WaterMeter> WaterMeters;
    public MetersPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        BindingContext= this;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var result = await _apiService.GetMeters();
        WaterMeters = result.Item1;
        MetersList.ItemsSource = WaterMeters;

    }

    private async void RequestBtn_Clicked(object sender, EventArgs e)
    {
        var response = await _apiService.RequestMeterByUser();
        if (response.ErrorMessage != null)
        {
            await DisplayAlert("Error", "Error requesting", "Cancel");
        }
        else
        {
            await DisplayAlert("Request", "Please wait while your request is accepted.", "OK");
        }
    }

    private async void MetersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tappedMeter = e.CurrentSelection.FirstOrDefault() as WaterMeter;

        var response = await _apiService.GetReadings(tappedMeter.Id);
        if (response.ErrorMessage != null)
        {
            await DisplayAlert("Error", "An error occurred while displaying the readings.", "Cancel");
        }
        else
        {
            await Navigation.PushAsync(new MeterReadingsPage(_apiService, response.Readings, tappedMeter.Id));
        }
        
    }
}