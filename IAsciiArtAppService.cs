using System.CommandLine.Parsing;
using System.CommandLine;

namespace AsciiArt
{
    public interface IAsciiArtAppService
    {
        public Task<int> InvokeAsync(string[] args);
    }
}