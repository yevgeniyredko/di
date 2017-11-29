using System;
using System.Drawing;
using System.Drawing.Imaging;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using CommandLine;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.ImageDrawer;
using TagsCloudContainer.TextColorGenerator;
using TagsCloudContainer.TextParser;

namespace TagsCloudContainer.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if (!Parser.Default.ParseArgumentsStrict(args, options))
            {
                var helpText = CommandLine.Text.HelpText.AutoBuild(options);
                Console.WriteLine(helpText);
                return;
            }

            var application = CreateApplication(options);
            application.Run(options.InputFile, options.OutputFile);
        }

        private static Application CreateApplication(Options options)
        {
            var container = new WindsorContainer()
                .AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<ImageSettings>().Instance(CreateImageSettings(options)),
                Component.For<ImageFormat>().Instance(GetImageFormat(options)),
                Component.For<IFileReader>().Instance(CreateFileReader(options)),
                Component.For<ITextParser>().ImplementedBy<SingleWordInLineTextParser>(),

                Component.For<ICloudLayouter>().ImplementedBy<CircularCloudLayouter>().LifestyleTransient(),
                Component.For<ICloudLayouterFactory>().AsFactory(),
                
                Component.For<FontSizeCalculator>().LifestyleTransient(),
                Component.For<IFontSizeCalculatorFactory>().AsFactory(),

                Component.For<BitmapDrawer>(),
                
                Component.For<Application>());

            return container.Resolve<Application>();
        }

        private static ImageFormat GetImageFormat(Options options)
        {
            switch (options.ImageFormat)
            {
                case OutputImageFormat.Png:
                    return ImageFormat.Png;
                case OutputImageFormat.Bmp:
                    return ImageFormat.Bmp;
                case OutputImageFormat.Gif:
                    return ImageFormat.Gif;
                case OutputImageFormat.Jpeg:
                    return ImageFormat.Jpeg;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IFileReader CreateFileReader(Options options)
        {
            switch (options.DocumentFormat)
            {
                case DocumentFormat.Txt:
                    return new TxtFileReader();
                case DocumentFormat.Docx:
                    return new DocxFileReader();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static ImageSettings CreateImageSettings(Options options)
        {
            return new ImageSettings(
                new Size(options.Width, options.Height), 
                Color.FromName(options.BackgroundColor),
                new FontFamily(options.Font),
                new SingleTextColorGenerator(Color.FromName(options.TextColor)));
        }
    }
}
