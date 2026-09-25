using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talk2Me.Data;
using Talk2Me.Models;
using Talk2Me.ViewModels;

public class AccountService : IAccountsService
{
    private readonly AppDbContext _context;

    private readonly PasswordHasher<User> _passwordHasher;

    /// <summary>
    /// Account Service Constructor. 
    /// Initializes the service with the provided AppDbContext.
    /// </summary>
    public AccountService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    /// <summary>
    /// Obtains a user from the database based on the provided ID.
    /// </summary>
    /// <param name="userId">User's ID</param>
    public async Task<User?> GetUserById(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    /// <summary>
    /// Obtains a user from the database based on the provided Username.
    /// </summary>
    /// <param name="username">User's Username</param>
    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    /// <summary>
    /// Checks if a User exists in the database based on the provided Username.
    /// </summary>
    /// <param name="username">User's Username</param>
    public async Task<bool> UsernameExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }

    /// <summary>
    /// Checks if a User exists in the database based on the provided Email.
    /// </summary>
    /// <param name="email">User's Email</param>
    /// <param name="excludeUserId"></param>
    public async Task<bool> EmailExists(string email, Guid? excludeUserId = null)
    {
        return await _context.Users.AnyAsync(u => u.Email == email && (!excludeUserId.HasValue || u.UserId != excludeUserId.Value));
    }

    /// <summary>
    /// Verifies if the provided password matches the stored password hash for the given user.
    /// </summary>
    /// <param name="user">User Object</param>
    /// <param name="password">Inserted Password</param>
    /// <returns>Password Validity</returns>
    public async Task<bool> VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

    /// <summary>
    /// Updates the Last Time the User was Online in the database to the current UTC time.
    /// </summary>
    /// <param name="userId">User's ID</param>
    public async Task UpdateLastTimeOnline(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.LastTimeOnline = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Creates a new User in the database based on the provided SignUpViewModel and profile picture.
    /// </summary>
    /// <param name="model">SignUp ViewModel</param>
    /// <param name="profilePicture">Profile Picture Byte Array</param>
    public async Task<User> CreateUser(SignUpViewModel model, byte[] profilePicture)
    {
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = model.Email,
            ProfilePic = profilePicture,
            Role = "User",
            IsSuspended = false,
            JoinDate = DateTimeOffset.UtcNow,
            LastTimeOnline = null
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Allows updating a User's Email and Password based on the provided SettingsViewModel.
    /// </summary>
    /// <param name="user">User Object</param>
    /// <param name="model">Settings ViewModel</param>
    public async Task UpdateUser(User user, SettingsViewModel model)
    {
        user.Email = model.Email;
        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
        }
        await _context.SaveChangesAsync();
    }
}
