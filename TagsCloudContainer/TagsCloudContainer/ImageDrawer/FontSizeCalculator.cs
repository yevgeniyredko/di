using System;

namespace TagsCloudContainer.ImageDrawer
{
    public class FontSizeCalculator
    {
        private const float minFontSize = 8;
        private const float maxFontSize = 72;

        private readonly int minWordCount;
        private readonly int maxWordCount;

        public FontSizeCalculator(int minWordCount, int maxWordCount)
        {
            this.minWordCount = minWordCount;
            this.maxWordCount = maxWordCount;
        }

        public int CalculateFontSize(int wordCount)
        {
            return (int) Math.Ceiling((wordCount - minWordCount)
                                      * (maxFontSize - minFontSize)
                                      / (maxWordCount - minWordCount)
                                      + minFontSize);
        }
    }
}