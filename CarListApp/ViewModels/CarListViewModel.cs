using CarListApp.Models;
using CarListApp.Services;
using CarListApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CarListApp.ViewModels;

public partial class CarListViewModel : BaseViewModel
{
    const string editButtonText = "Update Car";
    const string createButtonText = "Add Car";
    private readonly CarApiService carApiService;
    NetworkAccess accessType = Connectivity.Current.NetworkAccess;
    string message = string.Empty;

    public ObservableCollection<Car> Cars { get; private set; } = new();

    public CarListViewModel(CarApiService carApiService)
    {
        Title = "Car List";
        this.carApiService = carApiService;
        AddEditButtonText = createButtonText;
        //GetCarList().Wait();
    }

    [ObservableProperty]
    bool isRefreshing;
    [ObservableProperty]
    string make = string.Empty;
    [ObservableProperty]
    string model = string.Empty;
    [ObservableProperty]
    string vin = string.Empty;
    [ObservableProperty]
    string addEditButtonText = string.Empty;
    [ObservableProperty]
    int carId;

    [RelayCommand]
    async Task GetCarList()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            if (Cars.Any()) Cars.Clear();

            var cars = new List<Car>();
            //var cars = App.CarDBService.GetCars();

            if (accessType == NetworkAccess.Internet)
            {
                cars = await carApiService.GetCars();
            }
            else
            {
                cars = App.CarDBService.GetCars();
            }

            foreach (var car in cars) Cars.Add(car);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get cars: {ex.Message}");
            await ShowAlert("Failed to retrive list of cars");
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GetCarDetails(int id)
    {
        if (id == 0) return;

        await Shell.Current.GoToAsync($"{nameof(CarDetailsPage)}?Id={id}", true);
    }

    [RelayCommand]
    async Task AddCar()
    {
        if (string.IsNullOrEmpty(Make) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
        {
            await Shell.Current.DisplayAlert("Invalid Data", "Please insert valid data", "Ok");
            return;
        }

        var car = new Car
        {
            Make = Make,
            Model = Model,
            Vin = Vin
        };

        App.CarDBService.AddCar(car);
        await Shell.Current.DisplayAlert("Info", App.CarDBService.StatusMessage, "Ok");
        await GetCarList();
    }

    [RelayCommand]
    async Task SaveCar()
    {
        if (string.IsNullOrEmpty(Make) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
        {
            await ShowAlert("Please insert valid data");
            return;
        }

        var car = new Car
        {
            Id = CarId,
            Make = Make,
            Model = Model,
            Vin = Vin
        };

        if (CarId != 0)
        {
            //UPDATE
            if (accessType == NetworkAccess.Internet)
            {
                await carApiService.UpdateCar(CarId, car);
                message = carApiService.StatusMessage;
            }
            else
            {
                App.CarDBService.UpdateCar(car);
                message = App.CarDBService.StatusMessage;
            }

        }
        else
        {
            //ADD
            if (accessType == NetworkAccess.Internet)
            {
                await carApiService.AddCar(car);
                message = carApiService.StatusMessage;
            }
            else
            {
                App.CarDBService.AddCar(car);
                message = App.CarDBService.StatusMessage;
            }

            await ShowAlert(message);
            await GetCarList();
            ClearForm();
        }

        await GetCarList();
        ClearForm();

    }

    [RelayCommand]
    async Task DeleteCar(int id)
    {
        if (id == 0)
        {
            await ShowAlert("Please try again");
            return;
        }

        if (accessType == NetworkAccess.Internet)
        {
            await carApiService.DeleteCar(id);
            message = carApiService.StatusMessage;
        }
        else
        {
            App.CarDBService.DeleteCar(id);
            message = App.CarDBService.StatusMessage;
        }
        await ShowAlert(message);
        await GetCarList();
    }

    [RelayCommand]
    async Task SetEditMode(int id)
    {
        AddEditButtonText = editButtonText;
        CarId = id;
        Car car;
        if (accessType == NetworkAccess.Internet)
        {
            car = await carApiService.GetCar(id);
        }
        else
        {
            car = App.CarDBService.GetCar(id);
        }

        Make = car.Make;
        Model = car.Model;
        Vin = car.Vin;
    }

    [RelayCommand]
    void ClearForm()
    {
        AddEditButtonText = createButtonText;
        CarId = 0;
        Make = string.Empty;
        Model = string.Empty;
        Vin = string.Empty;
    }

    private async Task ShowAlert(string mess)
    {
        await Shell.Current.DisplayAlert("Info", message, "Ok");
    }
}
