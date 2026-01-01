using DisplayService;
using DisplayService.Models;
using Figgle;
using Figgle.Fonts;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace AsciiArt
{
    public class AsciiArtAppService : IAsciiArtAppService
    {
        private readonly IAsciiArtService _asciiArtService;
        private readonly IDisplayService _displayService;
        private readonly IThemeService _themeService;
        public AsciiArtAppService(IAsciiArtService asciiArtService, IDisplayService displayService, IThemeService themeService)
        {
            _asciiArtService = asciiArtService;
            _displayService = displayService;
            _themeService = themeService;
        }

        public async Task<int> InvokeAsync(string[] args)
        {
            var textArg = new Argument<string[]>("text")
            {
                Description = "The text to convert to ASCII art"
            };
            textArg.Arity = ArgumentArity.ZeroOrMore; // Allow zero arguments when using stdin

            var fontNameOption = new Option<string>("--font", "-f")
            {
                Description = "The font to use for ASCII art",
                DefaultValueFactory = _ => "Standard"
            };

            var stdinOption = new Option<bool>("--stdin", "-i")
            {
                Description = "Read text input from stdin instead of arguments"
            };

            var listFontsCommand = new Command("list-fonts", "List all available Figgle fonts");
            listFontsCommand.SetAction(_ =>
            {
                HandleListFonts();
            });

            var listThemesCommand = new Command("list-themes", "List all available themes");
            listThemesCommand.SetAction(_ =>
            {
                HandleListThemes();
            });

            var availableThemes = _themeService.GetAvailableThemeNames();
            var themeOption = new Option<string>("--theme", "-th")
            {
                Description = "Specify the theme",
                DefaultValueFactory = _ => "default" // Default theme
            };

            themeOption.Validators.Add(optionResult =>
            {
                string? themeValue = optionResult.GetValueOrDefault<string>();

                if (string.IsNullOrWhiteSpace(themeValue))
                {
                    return;
                }

                if (!availableThemes.Contains(themeValue, StringComparer.OrdinalIgnoreCase))
                {
                    optionResult.AddError($"Invalid theme '{themeValue}'. Allowed values: {string.Join(", ", availableThemes)}");
                }
            });

            var rootCommand = new RootCommand("ASCII Art Generator - Convert text to ASCII art using Figgle fonts")
            {
                textArg,
                fontNameOption,
                themeOption,
                stdinOption
            };

            rootCommand.Subcommands.Add(listFontsCommand);
            rootCommand.Subcommands.Add(listThemesCommand);

            rootCommand.SetAction((ParseResult parseResult, CancellationToken token) =>
            {
                string[] text = parseResult.GetValue(textArg) ?? Array.Empty<string>();
                string fontName = parseResult.GetValue(fontNameOption) ?? "Standard";
                string theme = parseResult.GetValue(themeOption) ?? "default";
                bool useStdin = parseResult.GetValue(stdinOption);

                return HandleAsciiArt(text, fontName, theme, useStdin, token);
            });

            ParseResult parseResult = rootCommand.Parse(args);
            return await parseResult.InvokeAsync();
        }

        private async Task<int> HandleAsciiArt(string[] text, string fontName, string theme, bool useStdin, CancellationToken cancellationToken)
        {
            // Apply the selected theme
            Theme selectedTheme = _themeService.GetThemeByName(theme);
            _displayService.ApplyTheme(selectedTheme);

            string input;

            if (useStdin)
            {
                // Read from stdin
                input = await Console.In.ReadToEndAsync(cancellationToken);
                input = input.Trim(); // Remove trailing newlines
            }
            else
            {
                // Use command line arguments
                if (text == null || text.Length == 0)
                {
                    _displayService.DisplayMessage("Error: No text provided. Use arguments or --stdin option.", SpectreConsoleColor.Red);
                    return 1;
                }
                input = string.Join(" ", text) + " ";
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                _displayService.DisplayMessage("Error: No text to convert.", SpectreConsoleColor.Red);
                return 1;
            }

            (string asciiArt, var font) = _asciiArtService.Render(input, fontName);
            _displayService.DisplayMessage(asciiArt);

            return 0;
        }

        private void HandleListFonts()
        {
            Console.WriteLine("Available Figgle fonts:");
            Console.WriteLine();

            var fontNames = _asciiArtService.GetAvailableFontNames().OrderBy(name => name).ToList();
            DisplayInColumns(fontNames, 4);

            Console.WriteLine();
            Console.WriteLine($"Total fonts available: {fontNames.Count}");
        }

        private void HandleListThemes()
        {
            Console.WriteLine("Available themes:");
            Console.WriteLine();

            var themeNames = _themeService.GetAvailableThemeNames().OrderBy(name => name).ToList();
            DisplayInColumns(themeNames, 4);

            Console.WriteLine();
            Console.WriteLine($"Total themes available: {themeNames.Count()}");
        }

        private static void DisplayInColumns(List<string> items, int columnCount)
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
    }
}