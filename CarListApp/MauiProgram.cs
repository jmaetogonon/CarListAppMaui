using CarListApp.Services;
using CarListApp.ViewModels;
using CarListApp.Views;
using Microsoft.Extensions.Logging;

namespace CarListApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "cars.db3");

        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<CarDBService>(s, dbPath));

        builder.Services.AddTransient<CarApiService>();

        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoadingPageViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginViewModel>();

        builder.Services.AddSingleton<CarListViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<CarDetailsViewModel>();
        builder.Services.AddTransient<CarDetailsPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
