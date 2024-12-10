using AquaApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AquaApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://aquaapi-emhgeegsg3epbdce.canadacentral-01.azurewebsites.net/";
        private readonly ILogger<ApiService> _logger;

        JsonSerializerOptions _serializerOptions;

        public ApiService(HttpClient httpClient, 
            ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<ApiResponse<bool>> Register
            (string firstName,
            string lastName,
            string username,
            string address,
            string phoneNumber)
        {
            var userInfo = new UserTemp()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Address = address,
                PhoneNumber = phoneNumber
            };

            var json = JsonSerializer.Serialize(userInfo, _serializerOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Users/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                };
            }
            return new ApiResponse<bool> { Data = true };
        }

        

        public async Task<ApiResponse<bool>> Login(string userName, string password)
        {
            try
            {
                var login = new Login()
                {
                    Username = userName,
                    Password = password
                };

                var json = JsonSerializer.Serialize(login, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Users/Login", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending HTTP request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

                Preferences.Set("isLogged", true);
                Preferences.Set("accesstoken", result!.AccessToken);

                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro no login : {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> Logout()
        {
            try
            {


                var response = await PostRequestWithAuth("api/Users/Logout", null);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }
                Preferences.Set("isLogged", false);
                Preferences.Set("accesstoken", string.Empty);
                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> TestAuthn()
        {
            try
            {


                var response = await PostRequestWithAuth("api/Users/TestAuthn", null);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> RecoveryPassword(string email)
        {
            try
            {
                var resetPassword = new ResetPassword()
                {
                    Email = email
                };
                var json = JsonSerializer.Serialize(resetPassword, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Users/RecoverPassword", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> ResetPassword(string email, string token, string newPassword)
        {
            try
            {
                var resetPassword = new ResetPassword()
                {
                    Email = email,
                    Token = token,
                    NewPassword = newPassword
                };
                var json = JsonSerializer.Serialize(resetPassword, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Users/ResetPassword", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }


        public async Task<ApiResponse<bool>> ChangeUser(string firstName,
            string lastName,
            string Address,
            string PhoneNumber)
        {
            try
            {
                var userInfo = new UserTemp()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Address = Address,
                    PhoneNumber = PhoneNumber
                };
                var json = JsonSerializer.Serialize(userInfo, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequestWithAuth("api/Users/ChangeUserDetails", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<(List<Reading>? Readings, string? ErrorMessage)> GetReadings(int meterId)
        {
            string endpoint = $"api/Meters/Readings?id={meterId}";
            return await GetAsync<List<Reading>>(endpoint);
        }

        public async Task<(List<Invoice>? Invoices, string? ErrorMessage)> GetInvoices()
        {
            string endpoint = $"api/Invoice/GetInvoices";
            return await GetAsync<List<Invoice>>(endpoint);
        }

        public async Task<(List<Alert>? Invoices, string? ErrorMessage)> GetAlerts()
        {
            string endpoint = $"api/Alert/Alerts";
            return await GetAsync<List<Alert>>(endpoint);
        }

        public async Task<ApiResponse<bool>> UploadUserImage(byte[] imageArray)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(imageArray), "image", "image.jpg");
                var response = await PostRequestWithAuth("api/Users/uploadimage", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                        ? "Unauthorized"
                        : $"Error sending HTTP Request: {response.StatusCode}";

                    _logger.LogError($"Error sending HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool> { ErrorMessage = errorMessage };
                }
                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading profile picture: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> RequestMeterByUser()
        {
            try
            {


                var response = await PostRequestWithAuth("api/Meters/RequestMeterByUser", null);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> AddUserReading(int meterId, double usageAmount)
        {
            try
            {
                var readingInfo = new AddReading()
                {
                    Id = meterId,
                    usageAmount = usageAmount
                };
                var json = JsonSerializer.Serialize(readingInfo, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequestWithAuth("api/Meters/AddReading", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<ApiResponse<bool>> Pay(int invoiceId)
        {
            try
            {
                var info = new InvoicePay()
                {
                    Id = invoiceId
                };
                var json = JsonSerializer.Serialize(info, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Invoice/Pay", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending a HTTP Request: {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
        {
            var enderecoUrl = _baseUrl + uri;
            try
            {
                var result = await _httpClient.PostAsync(enderecoUrl, content);
                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error sending a Post request to {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private async Task<HttpResponseMessage> PostRequestWithAuth(string uri, HttpContent content)
        {
            AddAuthorizationHeader();
            var enderecoUrl = _baseUrl + uri;
            try
            {
                var result = await _httpClient.PostAsync(enderecoUrl, content);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending a Post request to {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private void AddAuthorizationHeader()
        {
            var token = Preferences.Get("accesstoken", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<(UserTemp, string? ErrorMessage)> GetUserDetails()
        {
            var endpoint = $"api/Users/UserDetails";
            return await GetAsync<UserTemp>(endpoint);
        }

        public async Task<(ProfilePic, string? ErrorMessage)> GetProfilePic()
        {
            var endpoint = $"api/Users/GetProfilePic";
            return await GetAsync<ProfilePic>(endpoint);
        }

        public async Task<(List<WaterMeter>, string? ErrorMessage)> GetMeters()
        {
            var endpoint = $"api/Meters/UserMeters";
            return await GetAsync<List<WaterMeter>>(endpoint);
        }

        private async Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(_baseUrl + endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                    return (data ?? Activator.CreateInstance<T>(), null);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (default, errorMessage);
                    }

                    string generalErrorMessage = $"Error in request: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (default, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"HTTP Request error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);

            }
            catch (JsonException ex) 
            {
                string errorMessage = $"JSON deserialization error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Unexpected error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);
            }
        }
    }
}
