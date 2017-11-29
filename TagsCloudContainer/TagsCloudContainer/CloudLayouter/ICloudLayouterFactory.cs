using System.Drawing;

namespace TagsCloudContainer.CloudLayouter
{
    public interface ICloudLayouterFactory
    {
        ICloudLayouter Create(Point center);
    }
}