using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.FontSizeCalculator;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.TextColorGenerator;

namespace TagsCloudContainer.ImageDrawer
{
    public class BitmapDrawer
    {
        private readonly CloudImageSettings cloudImageSettings;
        private readonly ICloudLayouterFactory cloudLayouterFactory;

        private ITextColorGenerator ColorGenerator => cloudImageSettings.TextColorGenerator;
        private Size BitmapSize => cloudImageSettings.ImageSize;
        private FontFamily FontFamily => cloudImageSettings.FontFamily;
        private Color BackgroundColor => cloudImageSettings.BackgroundColor;
        private Point ImageCenter => new Point(BitmapSize.Width / 2, BitmapSize.Height / 2);
        private IFontSizeCalculatorFactory FontSizeCalculatorFactory => 
            cloudImageSettings.FontSizeCalculatorFactory;

        public BitmapDrawer(CloudImageSettings cloudImageSettings, ICloudLayouterFactory cloudLayouterFactory)
        {
            this.cloudImageSettings = cloudImageSettings;
            this.cloudLayouterFactory = cloudLayouterFactory;
        }

        public Bitmap DrawTags(IEnumerable<(string word, int count)> tags)
        {
            var tagsArray = tags.OrderByDescending(t => t.count).ToArray();
            var fontSizeCalculator =
                FontSizeCalculatorFactory.Create(tagsArray.Last().count, tagsArray[0].count);
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
                    if (!TryDrawWord(word, font, cloudLayouter, graphics, brush, rectangleSize).IsSuccess)
                        break;
                }
            }

            return bitmap;
        }

        private static Result<None> TryDrawWord(
            string word,
            Font font,
            ICloudLayouter cloudLayouter,
            Graphics graphics,
            Brush brush,
            SizeF rectangleSize)
        {
            return cloudLayouter.PutNextRectangle(Size.Ceiling(rectangleSize))
                .Then(rect => graphics.DrawString(word, font, brush, rect.X, rect.Y));
        }
    }
}