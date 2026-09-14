using Microsoft.AspNetCore.Mvc;
using Talk2Me.Data.Services;
using Talk2Me.Models;

namespace Talk2Me.Controllers
{
    public class ThemesController : Controller
    {
        /// <summary>
        /// Themes Service used to manage Themes in the database.
        /// </summary>
        private readonly IThemesService _themesService;

        /// <summary>
        /// Themes Controller Constructor.
        /// Initializes the controller with the provided IThemesService.
        /// </summary>
        public ThemesController(IThemesService themesService)
        {
            _themesService = themesService;
        }

        /// <summary>
        /// Shows the Themes Index View.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var themes = await _themesService.GetAllThemes();
            return View(themes);
        }

        /// <summary>
        /// Shows the Themes Create View.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new Theme and redirects to the Themes Index View.
        /// </summary>
        /// <param name="theme">Theme Object</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThemeId,ThemeName")] Theme theme)
        {
            if (ModelState.IsValid)
            {
                theme.ThemeId = Guid.NewGuid();
                await _themesService.CreateTheme(theme);
                return RedirectToAction(nameof(Index));
            }
            return View(theme);
        }

        /// <summary>
        /// Deletes a Theme and redirects to the Themes Index View.
        /// </summary>
        /// <param name="id">Theme's ID</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _themesService.DeleteTheme(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a Theme exists by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ThemeExists(Guid id)
        {
            return _themesService.GetThemeById(id).Result != null;
        }
    }
}
