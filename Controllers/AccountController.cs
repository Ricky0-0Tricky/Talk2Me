using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talk2Me.Models;
using Talk2Me.Utilities;
using Talk2Me.ViewModels;

public class AccountController : Controller
{
    /// <summary>
    /// Accounts Service used to manage Accounts in the database.
    /// </summary>
    private readonly IAccountsService _accountsService;

    /// <summary>
    /// Accounts Controller Constructor.
    /// Initializes the controller with the provided IAccountsService.
    /// </summary>
    public AccountController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    /// <summary>
    /// Shows the Login View.
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// Logins the Account and redirects to the Home View.
    /// </summary>
    /// <param name="username">Account's Username/param>
    /// <param name="password">Account's Password</param>
    /// <param name="rememberMe">Whether to remember the user for future logins</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe = false)
    {
        var user = await _accountsService.GetUserByUsername(username);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        if (user.IsSuspended)
        {
            ModelState.AddModelError("", "Your account has been suspended.");
            return View();
        }

        var passwordValid = await _accountsService.VerifyPassword(user, password);

        if (!passwordValid)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        await SignInUser(user, rememberMe);

        await _accountsService.UpdateLastTimeOnline(user.UserId);

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Logouts the Account and redirects to the Login View.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountsService.UpdateLastTimeOnline(GetCurrentUserId()!.Value);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// Shows the Sign Up View.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    /// <summary>
    /// Signs up a new User Account and redirects to the Login View.
    /// </summary>
    /// <param name="model">Sign Up ViewModel</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Check username
        if (await _accountsService.UsernameExists(model.UserName))
        {
            ModelState.AddModelError(nameof(model.UserName), "That username is already taken.");
            return View(model);
        }

        // Check email
        if (await _accountsService.EmailExists(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "That email address is already registered.");
            return View(model);
        }

        // Validate profile picture
        if (model.ProfilePic == null || model.ProfilePic.Length == 0)
        {
            ModelState.AddModelError(nameof(model.ProfilePic), "Please select a profile picture.");
            return View(model);
        }

        var imageResult = await ImageValidator.ValidateAndReadAsync(model.ProfilePic);

        if (!imageResult.IsValid)
        {
            ModelState.AddModelError(nameof(model.ProfilePic), imageResult.Error!);
            return View(model);
        }

        // Create user
        await _accountsService.CreateUser(model, imageResult.Data!);

        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// Shows the Settings View for the currently logged-in user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return RedirectToAction(nameof(Login));


        var user = await _accountsService.GetUserById(userId.Value);

        if (user == null)
            return NotFound();


        var model = new SettingsViewModel
        {
            Email = user.Email
        };

        return View(model);
    }

    /// <summary>
    /// Saves the updated settings for the currently logged-in user and refreshes the authentication cookie.
    /// </summary>
    /// <param name="model">Settings ViewModel</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(SettingsViewModel model)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var user = await _accountsService.GetUserById(userId.Value);

        if (user == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check whether email is already used
        if (await _accountsService.EmailExists(model.Email, user.UserId))
        {
            ModelState.AddModelError(nameof(model.Email), "That email address is already registered.");
            return View(model);
        }

        // Validate new profile picture if supplied
        byte[]? profilePicture = null;
        if (model.ProfilePic != null)
        {
            var imageResult = await ImageValidator.ValidateAndReadAsync(model.ProfilePic);

            if (!imageResult.IsValid)
            {
                ModelState.AddModelError(nameof(model.ProfilePic), imageResult.Error!);
                return View(model);
            }

            profilePicture = imageResult.Data;
        }

        // Update user
        await _accountsService.UpdateUser(user, model);

        // Refresh authentication cookie
        user.Email = model.Email;
        await SignInUser(user, rememberMe: true);
        TempData["SuccessMessage"] = "Your settings have been saved successfully.";
        return RedirectToAction(nameof(Settings));
    }

    /// <summary>
    /// Gets the currently logged-in user's ID from the authentication claims.
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
            return null;

        return userId;
    }

    /// <summary>
    /// Signs in the user by creating an authentication cookie with the user's claims.
    /// </summary>
    /// <param name="user">User Object</param>
    /// <param name="rememberMe">Whether to remember the user for future logins</param>
    private async Task SignInUser(User user, bool rememberMe)
    {
        var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.UserId.ToString()),

        new Claim(
            ClaimTypes.Name,
            user.UserName),

        new Claim(
            ClaimTypes.Email,
            user.Email),

        new Claim(
            ClaimTypes.Role,
            user.Role)
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,

            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
    }
}