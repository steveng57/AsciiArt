namespace AsciiArt
{
    public interface IAsciiArtService
    {
        (string, Figgle.FiggleFont) Render(string input, string fontName);
    }
}
