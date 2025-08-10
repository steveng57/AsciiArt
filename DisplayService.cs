using Spectre.Console;
using DisplayService.Models;

namespace DisplayService
{
    public class DisplayService : IDisplayService
    {
        private SpectreConsoleColor _messageColor = SpectreConsoleColor.Aqua;
        private SpectreConsoleColor _labelColor = SpectreConsoleColor.Green;
        private SpectreConsoleColor _valueColor = SpectreConsoleColor.Yellow;

        public void ApplyTheme(Theme theme)
        {
            _messageColor = theme.MessageColor;
            _labelColor = theme.LabelColor;
            _valueColor = theme.ValueColor;
        }

        public SpectreConsoleColor MessageColor
        {
            get => _messageColor;
            set => _messageColor = value;
        }

        public SpectreConsoleColor LabelColor
        {
            get => _labelColor;
            set => _labelColor = value;
        }

        public SpectreConsoleColor ValueColor
        {
            get => _valueColor;
            set => _valueColor = value;
        }

        public void DisplayMessage(string message, SpectreConsoleColor color)
        {
            AnsiConsole.MarkupLine($"[{GetColorString(color)}]{message}[/]");
        }

        public void DisplayMessage(string label, string value, SpectreConsoleColor labelColor, SpectreConsoleColor valueColor)
        {
            AnsiConsole.MarkupLine($"    [{GetColorString(labelColor)}]{label.PadRight(20)}[/][{GetColorString(valueColor)}]{value}[/]");
        }

        public void DisplayMessage(string message)
        {
            DisplayMessage(message, _messageColor);
        }

        public void DisplayMessage(string label, string value)
        {
            DisplayMessage(label, value, _labelColor, _valueColor);
        }

        public void DisplayHeader(string header)
        {
            AnsiConsole.MarkupLine($"[{GetColorString(_messageColor)}]{header}[/]");
        }

        public void DisplayHeader(string header, string value)
        {
            AnsiConsole.MarkupLine($"[{GetColorString(_messageColor)}]{header.PadRight(24)}[/][{GetColorString(_valueColor)}]{value}[/]");
        }

        public void SetMessageColor(SpectreConsoleColor color)
        {
            _messageColor = color;
        }

        public void SetLabelColor(SpectreConsoleColor color)
        {
            _labelColor = color;
        }

        public void SetValueColor(SpectreConsoleColor color)
        {
            _valueColor = color;
        }

        private string GetColorString(SpectreConsoleColor color)
        {
            return color.ToString().ToLower();
        }
    }
}
