using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudContainer.CloudLayouter;

namespace TagsCloudContainer.ImageDrawer
{
    public class BitmapDrawer
    {
        private readonly ImageSettings imageSettings;
        private readonly ICloudLayouterFactory cloudLayouterFactory;
        private readonly IFontSizeCalculatorFactory fontSizeCalculatorFactory;

        public BitmapDrawer(
            ImageSettings imageSettings, 
            ICloudLayouterFactory cloudLayouterFactory, 
            IFontSizeCalculatorFactory fontSizeCalculatorFactory)
        {
            this.imageSettings = imageSettings;
            this.cloudLayouterFactory = cloudLayouterFactory;
            this.fontSizeCalculatorFactory = fontSizeCalculatorFactory;
        }

        public Bitmap DrawTags(IEnumerable<(string word, int count)> tags)
        {
            var bitmapSize = imageSettings.ImageSize;
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
            var cloudLayouter = cloudLayouterFactory.Create(center);
            var tagsArray = tags.OrderByDescending(t => t.count).ToArray();
            var fontSizeCalculator =
                fontSizeCalculatorFactory.Create(tagsArray[tagsArray.Length - 1].count, tagsArray[0].count);
            var colorGenerator = imageSettings.TextColorGenerator;

            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(imageSettings.BackgroundColor))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.FillRectangle(brush, new Rectangle(Point.Empty, bitmapSize));

                foreach (var (word, count) in tagsArray)
                {
                    var fontSize = fontSizeCalculator.CalculateFontSize(count);
                    var font = new Font(imageSettings.FontFamily, fontSize);
                    brush.Color = colorGenerator.GetTextColor(fontSize);

                    var rectangleSizeF = graphics.MeasureString(word, font);
                    try
                    {
                        var layoutRectangle = cloudLayouter.PutNextRectangle(Size.Ceiling(rectangleSizeF));
                        graphics.DrawString(word, font, brush, layoutRectangle);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
            return bitmap;
        }
    }
}