using CarListApp.Models;
using CarListApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Web;

namespace CarListApp.ViewModels;

#nullable disable

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CarDetailsViewModel : BaseViewModel, IQueryAttributable
{

    private readonly CarApiService carApiService;
    NetworkAccess accessType = Connectivity.Current.NetworkAccess;

    public CarDetailsViewModel(CarApiService carApiService)
    {
        this.carApiService = carApiService;
    }

    [ObservableProperty]
    Car car;

    [ObservableProperty]
    int id;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Id = Convert.ToInt32(HttpUtility.UrlDecode(query["Id"].ToString()));

        //Id = Convert.ToInt32(HttpUtility.UrlDecode(query[nameof(Id)].ToString()));
        //Car = App.CarDBService.GetCar(Id);
    }

    public async Task GetCarData()
    {
        if (accessType == NetworkAccess.Internet)
        {
            Car = await carApiService.GetCar(Id);
        }
        else
        {
            Car = App.CarDBService.GetCar(Id);
        }
    }

}
