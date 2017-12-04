using System;
using System.Drawing;

namespace TagsCloudContainer.TextColorGenerator
{
    public class RandomTextColorGenerator : ITextColorGenerator
    {
        private readonly Random rnd = new Random();

        public Color GetTextColor(int fontSize)
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }
    }
}