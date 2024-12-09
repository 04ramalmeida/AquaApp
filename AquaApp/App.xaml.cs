using AquaApp.Pages;
using AquaApp.Services;

namespace AquaApp
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;
        public App(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            MainPage = new NavigationPage(new RegisterPage(_apiService));
        }
    }
}
