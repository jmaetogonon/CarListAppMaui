﻿using CarListApp.Models;
using CarListApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Security.Claims;

namespace CarListApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly CarApiService carApiService;
    public LoginViewModel(CarApiService carApiService)
    {
        this.carApiService = carApiService;
    }
    [ObservableProperty]
    string username = string.Empty;
    [ObservableProperty]
    string password = string.Empty;

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayLoginMessage("Invalid Login Attempt");
        }
        else
        {
            // Call API to attempt a login
            var loginModel = new LoginModel(username, password);

            var response = await carApiService.Login(loginModel);

            // display message
            await DisplayLoginMessage(carApiService.StatusMessage);

            if (!string.IsNullOrEmpty(response.Token))
            {
                // Store token in secure storage 
                await SecureStorage.SetAsync("Token", response.Token);

                // build a menu on the fly...based on the user role
                var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;

                var role = jsonToken.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Role))?.Value;

                App.UserInfo = new UserInfo()
                {
                    Username = Username,
                    Role = role
                };


                // navigate to app's main page
                await Shell.Current.GoToAsync($"{nameof(MainPage)}");

            }
            else
            {
                await DisplayLoginMessage("Invalid Login Attempt");
            }
        }
    }

    async Task DisplayLoginMessage(string message)
    {
        await Shell.Current.DisplayAlert("Login Attempt Result", message, "ok");
        Password = string.Empty;

    }
}