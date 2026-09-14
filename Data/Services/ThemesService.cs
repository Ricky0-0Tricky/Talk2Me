using Microsoft.EntityFrameworkCore;
using Talk2Me.Models;

namespace Talk2Me.Data.Services
{
    public class ThemesService : IThemesService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Themes Service Constructor. 
        /// Initializes the service with the provided AppDbContext.
        /// </summary>
        /// <param name="context"></param>
        public ThemesService(AppDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Obtains all themes from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Theme>> GetAllThemes()
        {
            var themes = await _context.Themes.ToListAsync();
            return themes;
        }

        /// <summary>
        /// Obtains a theme from the database based on the provided ID.
        /// </summary>
        /// <param name="themeId">Theme's ID</param>
        public async Task<Theme?> GetThemeById(Guid themeId) {
            var theme = await _context.Themes.FirstOrDefaultAsync(t => t.ThemeId == themeId);
            return theme;
        }

        /// <summary>
        /// Creates a new theme in the database. 
        /// The provided Theme object is added to the context and changes are saved asynchronously.
        /// </summary>
        /// <param name="theme">Theme Object</param>
        public async Task CreateTheme(Theme theme) {
            _context.Add(theme);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a theme from the database based on the provided ID. 
        /// If the theme is found, it is removed from the context and changes are saved to the database.
        /// </summary>
        /// <param name="themeId">Theme's ID</param>
        public async Task DeleteTheme(Guid themeId) {
            var theme = await _context.Themes.FirstOrDefaultAsync(t => t.ThemeId == themeId);
            if (theme != null)
            {
               _context.Remove(theme);
            }
            await _context.SaveChangesAsync();
        }
    }
}
