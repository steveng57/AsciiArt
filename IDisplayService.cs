using DisplayService.Models;

namespace DisplayService
{
    public interface IDisplayService
    {
        void DisplayMessage(string message);
        void DisplayMessage(string message, SpectreConsoleColor color);
        void DisplayMessage(string label, string value);
        void DisplayMessage(string label, string value, SpectreConsoleColor labelColor, SpectreConsoleColor valueColor);

        // Methods to set colors
        void SetMessageColor(SpectreConsoleColor color);
        void SetLabelColor(SpectreConsoleColor color);
        void SetValueColor(SpectreConsoleColor color);

        void ApplyTheme(Theme theme);
        void DisplayHeader(string header);
        void DisplayHeader(string header, string value);
    }
}
