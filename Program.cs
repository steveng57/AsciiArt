using AsciiArt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using DisplayService;

namespace AsciiArtApp;
class Program
{
    static async Task<int> Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        var commandLineService = host.Services.GetRequiredService<ICommandLineService>();
        return await commandLineService.InvokeAsync(args);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IAsciiArtService, FiggleAsciiArtService>();
                services.AddSingleton<ICommandLineService, CommandLineService>();
                services.AddSingleton<IDisplayService, DisplayService.DisplayService>();
                services.AddSingleton<IThemeService, ThemeService>();
            });
}