using CarListApp.Services;

namespace CarListApp
{
    public partial class App : Application
    {
        public static CarDBService CarDBService { get; private set; }
        public App(CarDBService carService)
        {
            InitializeComponent();

            MainPage = new AppShell();
            CarDBService = carService;
        }
    }
}
