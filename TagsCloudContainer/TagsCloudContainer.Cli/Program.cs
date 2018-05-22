﻿using System;
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
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.TextColorGenerator;
using TagsCloudContainer.TextParser;

namespace TagsCloudContainer.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var result =
                from options in ParseOptions(args)
                from application in CreateApplication(options)
                select application.Run(options.InputFile, options.OutputFile);

            result.Then(r => r).OnFail(Console.WriteLine);
        }

        private static Result<Options> ParseOptions(string[] args)
        {
            var options = new Options();

            return new Parser().ParseArguments(args, options)
                ? Result.Ok(options)
                : Result.Fail<Options>(CommandLine.Text.HelpText.AutoBuild(options));
        }

        private static Result<Application> CreateApplication(Options options)
        {
            return new WindsorContainer().AddFacility<TypedFactoryFacility>().AsResult()
                .Then(container => container.Register(
                    Component.For<IFileReader>().ImplementedBy(fileReaders[options.DocumentFormat]),
                    Component.For<ITextParser>().Instance(CreateTextParser(options)),
                    Component.For<ITextColorGenerator>().Instance(CreateTextColorGenerator(options)),
                    Component.For<ImageFormat>().Instance(imageFormats[options.ImageFormat]),

                    Component.For<ICloudLayouter>()
                        .ImplementedBy<CircularCloudLayouter>().LifestyleTransient(),
                    Component.For<ICloudLayouterFactory>().AsFactory(),
                    Component.For<IFontSizeCalculator>()
                        .ImplementedBy(fontSizeCalculators[options.TextScale]).LifestyleTransient(),
                    Component.For<IFontSizeCalculatorFactory>().AsFactory(),

                    Component.For<CloudImageSettings>().UsingFactoryMethod(kernel =>
                        CreateImageSettings(
                            options,
                            kernel.Resolve<ITextColorGenerator>(),
                            kernel.Resolve<IFontSizeCalculatorFactory>())),
                    
                    Component.For<BitmapDrawer>().LifestyleTransient(),
                    Component.For<Application>().LifestyleTransient()))
                .Then(container => container.Resolve<Application>());
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
                ? new SimpleTextParserWithoutBoringWords(options.BoringWords) 
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

        private static CloudImageSettings CreateImageSettings(
            Options options, 
            ITextColorGenerator colorGenerator,
            IFontSizeCalculatorFactory fontSizeCalculatorFactory)
        {
            return new CloudImageSettings(
                new Size(options.Width, options.Height),
                Color.FromKnownColor(options.BackgroundColor),
                new FontFamily(options.Font),
                colorGenerator, 
                fontSizeCalculatorFactory);
        }
    }
}