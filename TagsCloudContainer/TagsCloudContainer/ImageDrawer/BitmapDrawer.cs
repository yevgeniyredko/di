using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.FontSizeCalculator;
using TagsCloudContainer.TextColorGenerator;

namespace TagsCloudContainer.ImageDrawer
{
    public class BitmapDrawer
    {
        private readonly ImageSettings imageSettings;
        private readonly ICloudLayouterFactory cloudLayouterFactory;
        private readonly IFontSizeCalculatorFactory fontSizeCalculatorFactory;

        private ITextColorGenerator ColorGenerator => imageSettings.TextColorGenerator;
        private Size BitmapSize => imageSettings.ImageSize;
        private FontFamily FontFamily => imageSettings.FontFamily;
        private Color BackgroundColor => imageSettings.BackgroundColor;
        private Point ImageCenter => new Point(BitmapSize.Width / 2, BitmapSize.Height / 2);

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
            var tagsArray = tags.OrderByDescending(t => t.count).ToArray();
            var fontSizeCalculator =
                fontSizeCalculatorFactory.Create(tagsArray.Last().count, tagsArray[0].count);
            var cloudLayouter = cloudLayouterFactory.Create(ImageCenter);

            return DrawTags(tagsArray, fontSizeCalculator, cloudLayouter);
        }

        private Bitmap DrawTags(
            IEnumerable<(string word, int count)> tags,
            IFontSizeCalculator fontSizeCalculator,
            ICloudLayouter cloudLayouter)
        {
            var bitmap = new Bitmap(BitmapSize.Width, BitmapSize.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(BackgroundColor))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.FillRectangle(brush, new Rectangle(Point.Empty, BitmapSize));

                foreach (var (word, count) in tags)
                {
                    var fontSize = fontSizeCalculator.CalculateFontSize(count);
                    var font = new Font(FontFamily, fontSize);
                    brush.Color = ColorGenerator.GetTextColor(fontSize);

                    var rectangleSize = graphics.MeasureString(word, font);
                    if (!TryDrawWord(word, font, cloudLayouter, graphics, brush, rectangleSize))
                        break;
                }
            }

            return bitmap;
        }

        private static bool TryDrawWord(
            string word,
            Font font,
            ICloudLayouter cloudLayouter,
            Graphics graphics,
            Brush brush,
            SizeF rectangleSize)
        {
            try
            {
                var layoutRectangle = cloudLayouter.PutNextRectangle(Size.Ceiling(rectangleSize));
                graphics.DrawString(word, font, brush, layoutRectangle.X, layoutRectangle.Y);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}