using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DisplayService.Models
{
    public class Theme
    {
        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor MessageColor { get; set; }

        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor LabelColor { get; set; }

        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor ValueColor { get; set; }

        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor MessageBackgroundColor { get; set; } = SpectreConsoleColor.Black;

        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor LabelBackgroundColor { get; set; } = SpectreConsoleColor.Black;

        [JsonConverter(typeof(SpectreConsoleColorConverter))]
        public SpectreConsoleColor ValueBackgroundColor { get; set; } = SpectreConsoleColor.Black;

        // Parameterless constructor
        [JsonConstructor]
        public Theme() { }
    }

    public class SpectreConsoleColorConverter : JsonConverter<SpectreConsoleColor>
    {
        public override SpectreConsoleColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<SpectreConsoleColor>(reader.GetString(), true);
        }

        public override void Write(Utf8JsonWriter writer, SpectreConsoleColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}