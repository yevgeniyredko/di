using System.Drawing;
using CommandLine;
using CommandLine.Text;

namespace TagsCloudContainer.Cli.ConsoleOptions
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file")]
        public string OutputFile { get; set; }

        [Option('h', "height", Required = true, HelpText = "Output image height")]
        public int Height { get; set; }

        [Option('w', "width", Required = true, HelpText = "Output image width")]
        public int Width { get; set; }

        [Option("font", DefaultValue = "Arial", HelpText = "Font name")]
        public string Font { get; set; }

        [Option("bgcolor", DefaultValue = KnownColor.Black, HelpText = "Background color")]
        public KnownColor BackgroundColor { get; set; }

        [Option("textcolor", DefaultValue = KnownColor.White, 
            HelpText = "Text color")]
        public KnownColor TextColor { get; set; }

        [Option("textcolormode", DefaultValue = TextColorMode.Single,
            HelpText = "Text coloring algorithm (single/gradient/random)")]
        public TextColorMode TextColorMode { get; set; }

        [Option("scale", DefaultValue = TextScale.Linear,
            HelpText = "Text scale (linear/nonlinear)")]
        public TextScale TextScale { get; set; }

        [Option("docformat", DefaultValue = DocumentFormat.Txt, 
            HelpText = "Input file format (txt/docx)")]
        public DocumentFormat DocumentFormat { get; set; }

        [Option("imgformat", DefaultValue = OutputImageFormat.Png, 
            HelpText = "Output image format (png/bmp/gif/jpeg)")]
        public OutputImageFormat ImageFormat { get; set; }

        [OptionArray("boring", DefaultValue = new string[0], 
            HelpText = "Boring words")]
        public string[] BoringWords { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}