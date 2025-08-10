using Spectre.Console;
using DisplayService.Models;

namespace DisplayService
{
    public class DisplayService : IDisplayService
    {
        private SpectreConsoleColor _messageColor = SpectreConsoleColor.Aqua;
        private SpectreConsoleColor _labelColor = SpectreConsoleColor.Green;
        private SpectreConsoleColor _valueColor = SpectreConsoleColor.Yellow;
        private SpectreConsoleColor _messageBackgroundColor = SpectreConsoleColor.Black;
        private SpectreConsoleColor _labelBackgroundColor = SpectreConsoleColor.Black;
        private SpectreConsoleColor _valueBackgroundColor = SpectreConsoleColor.Black;

        public void ApplyTheme(Theme theme)
        {
            _messageColor = theme.MessageColor;
            _labelColor = theme.LabelColor;
            _valueColor = theme.ValueColor;
            _messageBackgroundColor = theme.MessageBackgroundColor;
            _labelBackgroundColor = theme.LabelBackgroundColor;
            _valueBackgroundColor = theme.ValueBackgroundColor;
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
            var colorString = _messageBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_messageColor) 
                : $"{GetColorString(_messageColor)} on {GetColorString(_messageBackgroundColor)}";
            AnsiConsole.MarkupLine($"[{colorString}]{message}[/]");
        }

        public void DisplayMessage(string label, string value)
        {
            var labelColorString = _labelBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_labelColor) 
                : $"{GetColorString(_labelColor)} on {GetColorString(_labelBackgroundColor)}";
            
            var valueColorString = _valueBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_valueColor) 
                : $"{GetColorString(_valueColor)} on {GetColorString(_valueBackgroundColor)}";

            AnsiConsole.MarkupLine($"    [{labelColorString}]{label.PadRight(20)}[/][{valueColorString}]{value}[/]");
        }

        public void DisplayHeader(string header)
        {
            var colorString = _messageBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_messageColor) 
                : $"{GetColorString(_messageColor)} on {GetColorString(_messageBackgroundColor)}";
            AnsiConsole.MarkupLine($"[{colorString}]{header}[/]");
        }

        public void DisplayHeader(string header, string value)
        {
            var messageColorString = _messageBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_messageColor) 
                : $"{GetColorString(_messageColor)} on {GetColorString(_messageBackgroundColor)}";
            
            var valueColorString = _valueBackgroundColor == SpectreConsoleColor.Black 
                ? GetColorString(_valueColor) 
                : $"{GetColorString(_valueColor)} on {GetColorString(_valueBackgroundColor)}";

            AnsiConsole.MarkupLine($"[{messageColorString}]{header.PadRight(24)}[/][{valueColorString}]{value}[/]");
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
