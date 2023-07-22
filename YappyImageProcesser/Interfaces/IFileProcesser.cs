using SixLabors.ImageSharp;

namespace YappyImageProcesser.Interfaces
{
    public interface IFileProcesser
    {
        public Image GetBackgroundImage(string fileLocation);
        public Task<Image> GetOverlayImage();
    }
}
