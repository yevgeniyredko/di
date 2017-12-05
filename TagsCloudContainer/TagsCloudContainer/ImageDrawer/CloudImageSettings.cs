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
            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
            TextColorGenerator = textColorGenerator;
            FontSizeCalculatorFactory = fontSizeCalculatorFactory;
        }
    }
}