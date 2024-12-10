namespace AquaApp.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		BindingContext = this;
		string about = $"Aqua Mobile App was created by {AppInfo.VersionString} and released on the 10th of December 2024.";
		AboutLabel.Text = about;
	}


}