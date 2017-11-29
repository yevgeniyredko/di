using System.Drawing;
using TagsCloudContainer.TextColorGenerator;

namespace TagsCloudContainer.ImageDrawer
{
    public class ImageSettings
    {
        public Size ImageSize { get; }
        public Color BackgroundColor { get; }
        public FontFamily FontFamily { get; }
        public ITextColorGenerator TextColorGenerator { get; }

        public ImageSettings(
            Size imageSize, 
            Color backgroundColor, 
            FontFamily fontFamily, 
            ITextColorGenerator textColorGenerator)
        {
            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
            TextColorGenerator = textColorGenerator;
        }
    }
}