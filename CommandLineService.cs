using System.CommandLine;
using System.CommandLine.Parsing;
using System.CommandLine.Builder;
using System.Reflection;
using Figgle;
using Figgle.Fonts;
using DisplayService;

namespace AsciiArt
{
    public class CommandLineService : ICommandLineService
    {
        private readonly IAsciiArtService _asciiArtService;
        private readonly IDisplayService _displayService;
        private readonly IThemeService _themeService;
        public CommandLineService(IAsciiArtService asciiArtService, IDisplayService displayService, IThemeService themeService)
        {
            _asciiArtService = asciiArtService;
            _displayService = displayService;
            _themeService = themeService;
        }

        public async Task<int> InvokeAsync(string[] args)
        {
            var textArg = new Argument<string[]>("text", "The text to convert to ASCII art");
            textArg.Arity = ArgumentArity.OneOrMore;
            
            var fontNameOption = new Option<string>(
                aliases: new[] { "--font", "-f" },
                description: "The font to use for ASCII art",
                getDefaultValue: () => "Standard"
            );

            var listFontsCommand = new Command("list-fonts", "List all available Figgle fonts");
            listFontsCommand.SetHandler(HandleListFonts);

            var availableThemes = _themeService.GetAvailableThemeNames();
            var themeOption = new Option<string>(
                new string[] { "--theme", "-th" },
                () => "default", // Default theme
                "Specify the theme").FromAmong(availableThemes.ToArray());


            var rootCommand = new RootCommand("ASCII Art Generator - Convert text to ASCII art using Figgle fonts")
            {
                textArg,
                fontNameOption,
                themeOption
            };

            rootCommand.AddCommand(listFontsCommand);
            rootCommand.SetHandler(HandleAsciiArt, textArg, fontNameOption);

            var parser = new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build();

            return await parser.InvokeAsync(args);
        }

        private void HandleAsciiArt(string[] text, string fontName)
        {
            string input = string.Join(" ", text);
            (string asciiArt, var font) = _asciiArtService.Render(input, fontName);
            _displayService.DisplayMessage(asciiArt);
        }

        private void HandleListFonts()
        {
            Console.WriteLine("Available Figgle fonts:");
            Console.WriteLine();

            var fontNames = GetAvailableFontNames().OrderBy(name => name).ToList();
            DisplayInColumns(fontNames, 4);

            Console.WriteLine();
            Console.WriteLine($"Total fonts available: {fontNames.Count}");
        }

        private static void DisplayInColumns(IList<string> items, int columnCount)
        {
            if (!items.Any()) return;

            // Calculate the maximum width needed for each column
            int maxLength = items.Max(item => item.Length);
            int columnWidth = maxLength + 2; // Add padding

            // Calculate rows needed
            int rows = (int)Math.Ceiling((double)items.Count / columnCount);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int index = row + (col * rows);
                    if (index < items.Count)
                    {
                        Console.Write(items[index].PadRight(columnWidth));
                    }
                }
                Console.WriteLine();
            }
        }

        private static IEnumerable<string> GetAvailableFontNames()
        {
            var properties = typeof(FiggleFonts).GetProperties(
                BindingFlags.Public | BindingFlags.Static);

            return properties
                .Where(prop => prop.PropertyType == typeof(FiggleFont))
                .Select(prop => prop.Name);
        }
    }
}
