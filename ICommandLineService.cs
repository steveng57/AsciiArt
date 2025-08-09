using System.CommandLine.Parsing;
using System.CommandLine;

namespace AsciiArt
{
    public interface ICommandLineService
    {
        public Task<int> InvokeAsync(string[] args);
    }
}
