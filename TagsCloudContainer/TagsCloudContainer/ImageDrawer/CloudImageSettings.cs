using System;
using System.Drawing;
using TagsCloudContainer.FontSizeCalculator;
using TagsCloudContainer.TextColorGenerator;

namespace TagsCloudContainer.ImageDrawer
{
    public class CloudImageSettings
    {
        public Size ImageSize { get; }
        public Color BackgroundColor { get; }
        public FontFamily FontFamily { get; }
        public ITextColorGenerator TextColorGenerator { get; }
        public IFontSizeCalculatorFactory FontSizeCalculatorFactory { get; }

        public CloudImageSettings(
            Size imageSize, 
            Color backgroundColor, 
            FontFamily fontFamily, 
            ITextColorGenerator textColorGenerator,
            IFontSizeCalculatorFactory fontSizeCalculatorFactory)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0) 
                throw new ArgumentException($"Image size must be positive, but was {imageSize}");

            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
            TextColorGenerator = textColorGenerator;
            FontSizeCalculatorFactory = fontSizeCalculatorFactory;
        }
    }
}