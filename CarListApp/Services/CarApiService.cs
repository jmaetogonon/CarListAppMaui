using CarListApp.Models;
using CarListApp.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace CarListApp.Services;

public class CarApiService
{
    HttpClient httpclient;

    public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8099" : "http://localhost:8099";

    public string StatusMessage = "";


    public CarApiService()
    {
        this.httpclient = new() { BaseAddress = new Uri(BaseAddress) };
    }

    public async Task<List<Car>> GetCars()
    {
        try
        {
            await SetAuthToken();
            var response = await httpclient.GetStringAsync("/cars");
            return JsonConvert.DeserializeObject<List<Car>>(response)!;
        }
        catch (Exception)
        {
            StatusMessage = "Failed to retrieve data.";
        }

        return null!;
    }
    public async Task<Car> GetCar(int id)
    {
        try
        {
            var response = await httpclient.GetStringAsync($"/cars/{id}");
            return JsonConvert.DeserializeObject<Car>(response)!;
        }
        catch (Exception)
        {
            StatusMessage = "Failed to retrieve data.";
        }

        return null!;
    }

    public async Task AddCar(Car car)
    {
        try
        {
            var response = await httpclient.PostAsJsonAsync($"/cars/", car);
            response.EnsureSuccessStatusCode();
            StatusMessage = "Insert Successful";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to insert data api: {ex.Message}";
        }
    }

    public async Task DeleteCar(int id)
    {
        try
        {
            var response = await httpclient.DeleteAsync($"/cars/{id}");
            response.EnsureSuccessStatusCode();
            StatusMessage = "Delete Successful";
        }
        catch (Exception)
        {
            StatusMessage = "Failed to delete data.";
        }
    }

    public async Task UpdateCar(int id, Car car)
    {
        try
        {
            var response = await httpclient.PutAsJsonAsync($"/cars/{id}", car);
            response.EnsureSuccessStatusCode();
            StatusMessage = "Update Successful";
        }
        catch (Exception)
        {
            StatusMessage = "Failed to update data.";
        }
    }

    public async Task<AuthReponseModel> Login(LoginModel loginModel)
    {
        try
        {
            var response = await httpclient.PostAsJsonAsync("/login", loginModel);
            response.EnsureSuccessStatusCode();
            StatusMessage = "Login Successful";

            return JsonConvert.DeserializeObject<AuthReponseModel>(await response.Content.ReadAsStringAsync())!;
        }
        catch (Exception ex)
        {
            StatusMessage = "Failed to login successfully.";
            return new AuthReponseModel();
        }
    }

    public async Task SetAuthToken()
    {
        var token = await SecureStorage.GetAsync("Token");
        httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}
