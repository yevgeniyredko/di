using System;

namespace TagsCloudContainer.FontSizeCalculator
{
    public class NonLinearFontSizeCalculator : IFontSizeCalculator
    {
        private const float minFontSize = 8;
        private float maxFontSize = 64;

        private int delta = 8;

        private readonly int minWordCount;
        private readonly int maxWordCount;

        public NonLinearFontSizeCalculator(int minWordCount, int maxWordCount)
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

            const float minMaxFontSize = 32;
            const int minDelta = 1;
            const int deltaDivider = 2;
            
            if (maxFontSize > minMaxFontSize)
                maxFontSize -= delta;
            if (delta > minDelta)
                delta /= deltaDivider;

            return result;
        }
    }
}