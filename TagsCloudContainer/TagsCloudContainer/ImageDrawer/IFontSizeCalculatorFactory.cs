namespace TagsCloudContainer.ImageDrawer
{
    public interface IFontSizeCalculatorFactory
    {
        FontSizeCalculator Create(int minWordCount, int maxWordCount);
    }
}