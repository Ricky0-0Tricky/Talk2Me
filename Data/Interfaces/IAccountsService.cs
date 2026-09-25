using Talk2Me.Models;
using Talk2Me.ViewModels;

public interface IAccountsService
{
    Task<User?> GetUserById(Guid userId);
    Task<User?> GetUserByUsername(string username);
    Task<bool> UsernameExists(string username);
    Task<bool> EmailExists(string email, Guid? excludeUserId = null);
    Task<bool> VerifyPassword(User user, string password);
    Task UpdateLastTimeOnline(Guid userId);
    Task<User> CreateUser(SignUpViewModel model, byte[] profilePicture);
    Task UpdateUser(User user, SettingsViewModel model);
}
