using AquaApp.Models;
using AquaApp.Services;
using Microsoft.Maui.Controls.Platform;

namespace AquaApp.Pages;

public partial class MyAccount : ContentPage
{
	private readonly ApiService _apiService;
    private UserTemp UserInfo;
    public MyAccount(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        BindingContext = this;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var userInfo = await _apiService.GetUserDetails();
        if (userInfo.ErrorMessage != null)
        {
            await DisplayAlert("Error", "Error getting user details", "Cancel");
        }
        else
        {
            FirstNameEntry.Text = userInfo.Item1.FirstName;
            LastNameEntry.Text = userInfo.Item1.LastName;
            AddressEntry.Text = userInfo.Item1.Address;
            PhoneNumberEntry.Text = userInfo.Item1.PhoneNumber;
            UsernameLabel.Text = userInfo.Item1.Username;
        }
        ProfilePic.Source = await GetProfilePicAsync();
        
    }

    private async Task<string?> GetProfilePicAsync()
    {
        string defaultImage = "avatar.jpg";
        var (response, errorMessage) = await _apiService.GetProfilePic();

        if (errorMessage != null) 
        {
            await DisplayAlert("Error", errorMessage ?? "We weren't able to get the profile picture.", "OK");
        }

        if (response.Url != null)
        {
            return response.Url;
        }
        return defaultImage;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        
        DisplayAlert("Hi", "Hi", "OK");
    }

    private void EditBtn_Clicked(object sender, EventArgs e)
    {
        FirstNameEntry.IsEnabled = true;
        LastNameEntry.IsEnabled= true;
        AddressEntry.IsEnabled = true;
        PhoneNumberEntry.IsEnabled = true;
        EditBtn.IsEnabled = false;
        SaveBtn.IsEnabled = true;
        
    }

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(FirstNameEntry.Text) || string.IsNullOrEmpty(LastNameEntry.Text) || string.IsNullOrEmpty(AddressEntry.Text) || string.IsNullOrEmpty(PhoneNumberEntry.Text))
        {
            await DisplayAlert("Error", "Please make sure none of the text entries are empty.", "Cancel");
            return;
        }

        var response = await _apiService.ChangeUser(FirstNameEntry.Text, LastNameEntry.Text, AddressEntry.Text, PhoneNumberEntry.Text);

        if (!response.HasError)
        {
            await DisplayAlert("Success", "Success", "Sucess");
            await Navigation.PushAsync(new MyAccount(_apiService));
        }
        else
        {
            await DisplayAlert("Error", response.ErrorMessage, "Cancel");
        }
    }

    private async void ProfilePic_Clicked(object sender, EventArgs e)
    {
        try
        {
            var profilePicArray = await PickImageAsync();
            if (profilePicArray is null)
            {
                await DisplayAlert("Error", "We were unable to load the image", "OK");
                return;
            }
            var response = await _apiService.UploadUserImage(profilePicArray);

            if (response.Data)
            {
                ProfilePic.Source = ImageSource.FromStream(() => new MemoryStream(profilePicArray));
                await DisplayAlert("Success", "Your profile picture has been set!", "OK");
            }
            else
            {
                await DisplayAlert("Error", response.ErrorMessage ?? "Unknown Error", "Cancel");
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Unknown error: {ex.Message}", "OK");
        }
    }

    private async Task<byte[]?> PickImageAsync()
    {
        try
        {
            var file = await MediaPicker.PickPhotoAsync();

            if (file == null)
            {
                return null;
            }

            using (var stream = await file.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "Feature not available in this device.", "OK");

        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Permission unavailable", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error picking the image: {ex.Message}", "OK");
        }
        return null;
    }
}