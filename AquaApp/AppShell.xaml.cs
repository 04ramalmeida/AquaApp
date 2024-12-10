using AquaApp.Pages;
using AquaApp.Services;

namespace AquaApp
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;
        public AppShell(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            ConfigureShell();
        }

        private void ConfigureShell()
        {
            var metersPage = new MetersPage(_apiService);
            var invoicesPage = new InvoicesPage(_apiService);
            var alertsPage = new AlertsPage(_apiService);
            var userMenuPage = new UserMenuPage(_apiService);

            Items.Add(new TabBar
            {
                Items =
                {
                    new ShellContent{Title="Meters",Icon = "meters", Content =metersPage },
                    new ShellContent{Title="Invoices",Icon = "invoices", Content =invoicesPage },
                    new ShellContent{Title="Alerts",Icon = "alerts", Content =alertsPage},
                    new ShellContent{Title="User",Icon = "user", Content =userMenuPage },

                }
            });
        }

        


    }
}
