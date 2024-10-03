using System.IdentityModel.Tokens.Jwt;

namespace CarListApp.ViewModels;

public partial class LoadingPageViewModel : BaseViewModel
{
    public LoadingPageViewModel()
    {
        CheckUserLoginDetails();
    }

    private async void CheckUserLoginDetails()
    {
        //retrieve token from internal storage
        var token = await SecureStorage.GetAsync("Token");
        if (string.IsNullOrEmpty(token))
        {
            await GotoLoginPage();
        }
        else
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            //EVALUATE TOKEN AND DECIDE IF VALID
            if (jsonToken?.ValidTo < DateTime.UtcNow)
            {
                SecureStorage.Remove("Token");
                await GotoLoginPage();
            }
            else
            {
                await GotoMainPage();
            }
        }
    }

    private async Task GotoLoginPage()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
    private async Task GotoMainPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }
}
