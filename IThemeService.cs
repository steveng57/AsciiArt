using DisplayService.Models;
namespace DisplayService
{
    public interface IThemeService
    {
        Theme GetThemeByName(string name);
        IEnumerable<string> GetAvailableThemeNames();
    }
}