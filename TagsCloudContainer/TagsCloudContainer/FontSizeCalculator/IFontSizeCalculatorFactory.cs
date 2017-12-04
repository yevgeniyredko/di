namespace TagsCloudContainer.FontSizeCalculator
{
    public interface IFontSizeCalculatorFactory
    {
        IFontSizeCalculator Create(int minWordCount, int maxWordCount);
    }
}