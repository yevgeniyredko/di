using System;
using System.Drawing.Imaging;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.ImageDrawer;
using TagsCloudContainer.Infrastructure;
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
            this.bitmapDrawer = bitmapDrawer;
            this.fileReader = fileReader;
            this.textParser = textParser;
            this.imageFormat = imageFormat;
        }

        public Result<None> Run(string inputFile, string outputFile)
        {
            return fileReader.ReadLines(inputFile)
                .Then(textParser.GetAllWords)
                .Then(bitmapDrawer.DrawTags)
                .Then(bmp => bmp.Save(outputFile, imageFormat));
        }
    }
}