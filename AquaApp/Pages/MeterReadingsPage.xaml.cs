using AquaApp.Models;
using AquaApp.Services;

namespace AquaApp.Pages;

public partial class MeterReadingsPage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly List<Reading> _readings;
    public MeterReadingsPage(ApiService apiService, List<Reading> readings)
    {
        InitializeComponent();
        _apiService = apiService;
        _readings = readings;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_readings != null)
        {
            ReadingList.ItemsSource = _readings;
            NoneLabel.IsVisible = false;
            ReadingList.IsVisible = true;
        }
        

    }

    private async void AddBtn_Clicked(object sender, EventArgs e)
    {
        var meterId = _readings.FirstOrDefault().WaterMeter.Id;
        await Navigation.PushAsync(new AddReadingPage(_apiService, meterId));
    }
}