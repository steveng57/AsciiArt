using System;
using Figgle;
using Figgle.Fonts;
using System.Reflection;

namespace AsciiArt
{
    public class FiggleAsciiArtService : IAsciiArtService
    {
        public (string,Figgle.FiggleFont) Render(string input, string fontName)
        {
            var font = GetFontByName(fontName);
            return (font.Render(input), font);
        }

        public IEnumerable<string> GetAvailableFontNames()
        {
            var properties = typeof(FiggleFonts).GetProperties(
                BindingFlags.Public | BindingFlags.Static);

            return properties
                .Where(prop => prop.PropertyType == typeof(FiggleFont))
                .Select(prop => prop.Name);
        }

        private static FiggleFont GetFontByName(string fontName)
        {
            // Use reflection to get the static property by name
            var property = typeof(FiggleFonts).GetProperty(fontName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase);
            if (property != null && property.GetValue(null) is FiggleFont font)
            {
               return font;
            }
            // Fallback to Standard if not found
            var myFont = FiggleFonts.Standard;
            return myFont;
        }
    }
}
