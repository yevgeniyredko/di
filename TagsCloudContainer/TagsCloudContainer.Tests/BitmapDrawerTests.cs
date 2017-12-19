using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.FontSizeCalculator;
using TagsCloudContainer.ImageDrawer;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.TextColorGenerator;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class BitmapDrawerTests
    {
        private const int fontSize = 16;
        private readonly Color textColor = Color.White;

        private CloudImageSettings imageSettings;
        private BitmapDrawer bitmapDrawer;

        private IFontSizeCalculator fontSizeCalculator;
        private IFontSizeCalculatorFactory fontSizeCalculatorFactory;
        private ICloudLayouter cloudLayouter;
        private ICloudLayouterFactory cloudLayouterFactory;

        [SetUp]
        public void SetUp()
        {
            var textColorGenerator = Substitute.For<ITextColorGenerator>();
            textColorGenerator.GetTextColor(Arg.Any<int>()).Returns(textColor);

            fontSizeCalculator = Substitute.For<IFontSizeCalculator>();
            fontSizeCalculator.CalculateFontSize(Arg.Any<int>()).Returns(fontSize);
            fontSizeCalculatorFactory = Substitute.For<IFontSizeCalculatorFactory>();
            fontSizeCalculatorFactory.Create(Arg.Any<int>(), Arg.Any<int>()).Returns(fontSizeCalculator);

            imageSettings = new CloudImageSettings(
                new Size(500, 500),
                Color.Black,
                new FontFamily("Arial"),
                textColorGenerator,
                fontSizeCalculatorFactory);

            cloudLayouter = Substitute.For<ICloudLayouter>();
            cloudLayouter.PutNextRectangle(Arg.Any<Size>())
                .Returns(c => new Rectangle(Point.Empty, c.Arg<Size>()).AsResult());
            cloudLayouterFactory = Substitute.For<ICloudLayouterFactory>();
            cloudLayouterFactory.Create(Arg.Any<Point>()).Returns(cloudLayouter);

            bitmapDrawer = new BitmapDrawer(imageSettings, cloudLayouterFactory);
        }

        [Test]
        public void DrawTags_DrawsCorrectBitmap()
        {
            const string word = "hello";
            var expectedBitmap = DrawWord(word);

            var actualBitmap = bitmapDrawer.DrawTags(new[] {(word, 1)});

            for (var x = 0; x < imageSettings.ImageSize.Width; x++)
            {
                for (var y = 0; y < imageSettings.ImageSize.Height; y++)
                {
                    actualBitmap.GetPixel(x, y).Should().Be(expectedBitmap.GetPixel(x, y));
                }
            }
        }

        [Test, TestCaseSource(nameof(tagsArrays))]
        public void DrawTags_CallsFontSizeCalculatorFactory_WithCorrectArguments((string word, int count)[] tags)
        {
            var minWordCount = tags.Select(t => t.count).Min();
            var maxWordCount = tags.Select(t => t.count).Max();

            bitmapDrawer.DrawTags(tags);

            fontSizeCalculatorFactory.Received(1).Create(minWordCount, maxWordCount);
        }

        [Test, TestCaseSource(nameof(tagsArrays))]
        public void DrawTags_CallsFontSizeCalculator_WithCorrectArgument((string word, int count)[] tags)
        {
            var sizes = tags.Select(t => t.count).Distinct();
            var statistics = sizes.Select(size => (size, tags.Count(t => t.count == size)));

            bitmapDrawer.DrawTags(tags);

            foreach (var (size, count) in statistics)
            {
                fontSizeCalculator.Received(count).CalculateFontSize(size);
            }
        }

        [Test]
        public void DrawTags_CallsCloudLayouterFactory_WithCorrectArgument()
        {
            var center = new Point(imageSettings.ImageSize.Width / 2, imageSettings.ImageSize.Height / 2);

            bitmapDrawer.DrawTags(new[] {("word", 1)});

            cloudLayouterFactory.Received(1).Create(center);
        }

        [Test]
        public void DrawTags_CallsCloudLayouter_WithCorrectArgument()
        {
            bitmapDrawer.DrawTags(new[] {("", 1)});

            cloudLayouter.Received(1).PutNextRectangle(Size.Empty);
        }

        private static TestCaseData[] tagsArrays =
        {
            new TestCaseData(new[] {("hello", 1)}).SetName("One tag"),
            new TestCaseData(new[] {("hello", 1), ("world", 5)}).SetName(@"Two tags with different count")
        };

        private Bitmap DrawWord(string word)
        {
            var bitmap = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(imageSettings.BackgroundColor))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.FillRectangle(brush, new Rectangle(Point.Empty, imageSettings.ImageSize));

                brush.Color = textColor;
                var font = new Font(imageSettings.FontFamily, fontSize);

                graphics.DrawString(word, font, brush, Point.Empty);
            }

            return bitmap;
        }
    }
}