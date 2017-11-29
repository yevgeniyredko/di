using System.Drawing.Imaging;
using System.Linq;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.ImageDrawer;
using TagsCloudContainer.TextParser;

namespace TagsCloudContainer.Cli
{
    class Application
    {
        private readonly IFileReader fileReader;
        private readonly ITextParser textParser;
        private readonly BitmapDrawer bitmapDrawer;
        private readonly ImageFormat imageFormat;

        public Application(
            IFileReader fileReader, 
            ITextParser textParser, 
            BitmapDrawer bitmapDrawer, 
            ImageFormat imageFormat)
        {
            this.fileReader = fileReader;
            this.textParser = textParser;
            this.bitmapDrawer = bitmapDrawer;
            this.imageFormat = imageFormat;
        }

        public void Run(string inputFile, string outputFile)
        {
            var input = fileReader.ReadLines(inputFile);
            var parsed = textParser.GetAllWords(input);
            var bmp = bitmapDrawer.DrawTags(parsed.ToArray());

            bmp.Save(outputFile, imageFormat);
        }
    }
}