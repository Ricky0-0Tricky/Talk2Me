using Talk2Me.Models;

namespace Talk2Me.Data.Interfaces
{
    public interface IThemesService
    {
        Task<IEnumerable<Theme>> GetAllThemes();
        Task<Theme?> GetThemeById(Guid themeId);
        Task CreateTheme(Theme theme);
        Task DeleteTheme(Guid themeId);
    }
}
