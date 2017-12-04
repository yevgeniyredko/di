using System;

namespace TagsCloudContainer.FontSizeCalculator
{
    public class LinearFontSizeCalculator : IFontSizeCalculator
    {
        private const float minFontSize = 8;
        private const float maxFontSize = 64;

        private readonly int minWordCount;
        private readonly int maxWordCount;

        public LinearFontSizeCalculator(int minWordCount, int maxWordCount)
        {
            this.minWordCount = minWordCount;
            this.maxWordCount = maxWordCount;
        }

        public int CalculateFontSize(int wordCount)
        {
            var result = (int) Math.Ceiling((wordCount - minWordCount)
                                            * (maxFontSize - minFontSize)
                                            / (maxWordCount - minWordCount)
                                            + minFontSize);
            return result;
        }
    }
}