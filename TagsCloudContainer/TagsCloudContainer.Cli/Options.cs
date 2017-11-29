using CommandLine;

namespace TagsCloudContainer.Cli
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

        [Option("bgcolor", DefaultValue = "Black", HelpText = "Background color")]
        public string BackgroundColor { get; set; }

        [Option("txtcolor", DefaultValue = "White", HelpText = "Text color")]
        public string TextColor { get; set; }

        [Option("font", DefaultValue = "Arial", HelpText = "Font name")]
        public string Font { get; set; }

        [Option("docformat", DefaultValue = DocumentFormat.Txt, 
            HelpText = "Input file format (txt/docx)")]
        public DocumentFormat DocumentFormat { get; set; }

        [Option("imgformat", DefaultValue = OutputImageFormat.Png, 
            HelpText = "Output image format (png/bmp/gif/jpeg)")]
        public OutputImageFormat ImageFormat { get; set; }
    }
}