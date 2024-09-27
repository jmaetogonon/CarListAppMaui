using CarListApp.Models;
using Newtonsoft.Json;
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
}
