using CarListApp.Models;
using CarListApp.Services;

namespace CarListApp;
#nullable disable
public partial class App : Application
{
    public static UserInfo UserInfo;

    public static CarDBService CarDBService { get; private set; }
    public App(CarDBService carService)
    {
        InitializeComponent();

        MainPage = new AppShell();
        CarDBService = carService;
    }
}
