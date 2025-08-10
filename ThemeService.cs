using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DisplayService.Models;

namespace DisplayService
{
    [JsonSerializable(typeof(Dictionary<string, Dictionary<string, Theme>>))]
    public partial class ThemeJsonContext : JsonSerializerContext
    {
    }

    public class ThemeService : IThemeService
    {
        private readonly Dictionary<string, Theme> _themeDictionary;

        public ThemeService()
        {
            _themeDictionary = LoadThemes();
        }

        private Dictionary<string, Theme> LoadThemes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AsciiArt.themes.json";

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Embedded resource not found: " + resourceName);
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();

                    var themes = JsonSerializer.Deserialize(json, ThemeJsonContext.Default.DictionaryStringDictionaryStringTheme);

                    if (themes == null || !themes.ContainsKey("Themes"))
                    {
                        throw new InvalidOperationException("Invalid themes.json format.");
                    }

                    var themeDictionary = new Dictionary<string, Theme>(StringComparer.OrdinalIgnoreCase);
                    foreach (var theme in themes["Themes"])
                    {
                        themeDictionary[theme.Key] = theme.Value;
                    }

                    return themeDictionary;
                }
            }
        }

        public Theme GetThemeByName(string name)
        {
            return _themeDictionary.TryGetValue(name, out var theme) ? theme : _themeDictionary["Default"];
        }

        public IEnumerable<string> GetAvailableThemeNames()
        {
            return _themeDictionary.Keys.Select(key => key.ToLower()).ToArray();
        }
    }
}
