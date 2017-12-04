using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using CommandLine;
using TagsCloudContainer.Cli.ConsoleOptions;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.FontSizeCalculator;
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
                Component.For<IFileReader>().ImplementedBy(fileReaders[options.DocumentFormat]),

                Component.For<ITextParser>().Instance(CreateTextParser(options)),
                Component.For<ITextColorGenerator>().Instance(CreateTextColorGenerator(options)),
                Component.For<ImageFormat>().Instance(imageFormats[options.ImageFormat]),

                Component.For<ImageSettings>().UsingFactoryMethod(kernel => 
                    CreateImageSettings(options, kernel.Resolve<ITextColorGenerator>())),

                Component.For<IFontSizeCalculator>()
                    .ImplementedBy(fontSizeCalculators[options.TextScale]).LifestyleTransient(),
                Component.For<IFontSizeCalculatorFactory>().AsFactory(),

                Component.For<ICloudLayouter>()
                    .ImplementedBy<CircularCloudLayouter>().LifestyleTransient(),
                Component.For<ICloudLayouterFactory>().AsFactory(),

                Component.For<BitmapDrawer>().LifestyleTransient(),

                Component.For<Application>().LifestyleTransient());

            return container.Resolve<Application>();
        }

        private static readonly Dictionary<OutputImageFormat, ImageFormat> imageFormats
            = new Dictionary<OutputImageFormat, ImageFormat>
            {
                [OutputImageFormat.Bmp] = ImageFormat.Bmp,
                [OutputImageFormat.Gif] = ImageFormat.Gif,
                [OutputImageFormat.Jpeg] = ImageFormat.Jpeg,
                [OutputImageFormat.Png] = ImageFormat.Png
            };

        private static readonly Dictionary<DocumentFormat, Type> fileReaders
            = new Dictionary<DocumentFormat, Type>
            {
                [DocumentFormat.Txt] = typeof(TxtFileReader),
                [DocumentFormat.Docx] = typeof(DocxFileReader)
            };
        
        private static readonly Dictionary<TextScale, Type> fontSizeCalculators
            = new Dictionary<TextScale, Type>
            {
                [TextScale.Linear] = typeof(LinearFontSizeCalculator),
                [TextScale.NonLinear] = typeof(NonLinearFontSizeCalculator)
            };


        private static ITextParser CreateTextParser(Options options)
        {
            return options.BoringWords.Length > 0 
                ? new SimpleTextParserWithCustomBoringWords(options.BoringWords) 
                : new SimpleTextParser();
        }

        private static ITextColorGenerator CreateTextColorGenerator(Options options)
        {
            if (options.TextColorMode == TextColorMode.Random)
                return new RandomTextColorGenerator();

            if (options.TextColorMode == TextColorMode.Single)
                return new SingleTextColorGenerator(Color.FromKnownColor(options.TextColor));

            return new GradientTextColorGenerator(Color.FromKnownColor(options.TextColor));    
        }

        private static ImageSettings CreateImageSettings(Options options, ITextColorGenerator colorGenerator)
        {
            return new ImageSettings(
                new Size(options.Width, options.Height),
                Color.FromKnownColor(options.BackgroundColor),
                new FontFamily(options.Font),
                colorGenerator);
        }
    }
}