using System.CommandLine;
using System.CommandLine.Parsing;
using System.CommandLine.Builder;
using System.Reflection;
using Figgle;
using Figgle.Fonts;

namespace AsciiArt
{
    public class CommandLineService : ICommandLineService
    {
        private readonly IAsciiArtService _asciiArtService;

        public CommandLineService(IAsciiArtService asciiArtService)
        {
            _asciiArtService = asciiArtService;
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

            var rootCommand = new RootCommand("ASCII Art Generator - Convert text to ASCII art using Figgle fonts")
            {
                textArg,
                fontNameOption
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
            Console.WriteLine(asciiArt);    
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
