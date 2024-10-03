namespace CarListApp.Models;

public class LoginModel
{
    public LoginModel(string username, string pass)
    {
        Username = username;
        Password = pass;
    }
    public string Username { get; set; }
    public string Password { get; set; }
}