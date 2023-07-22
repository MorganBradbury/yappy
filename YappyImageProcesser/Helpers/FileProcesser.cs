using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using YappyImageProcesser.Interfaces;

namespace YappyImageProcesser.Helpers
{
    public class FileProcesser : IFileProcesser
    {
        // Future improvement, pass in the backgroundImageUrl so its not stored within this class. It should be passed in through the API, but kept it hard-coded for code challenge
        private readonly string _backgroundImageUrl = "Images/Background.jpg";
        private readonly string _overlayImageUrl = "https://images.yappy.com/yappicon/greatdane/greatdane-04.png";
        

        public Image GetBackgroundImage(string fileLocation)
        {
            // Return image
            return Image.Load<Rgba32>(GetFileBytes(fileLocation));
        }

        private byte[] GetFileBytes(string fileLocation)
        {
            // Switch statement below allows for the ability to load in different images from additional data stores later.
            byte[] backgroundImageBytes = Array.Empty<byte>();
            switch (fileLocation)
            {
                case "Local":
                    // If locally, read all bytes from local file.
                    backgroundImageBytes = File.ReadAllBytes(_backgroundImageUrl);
                    break;
                case "S3":
                    // Load from S3
                    throw new NotImplementedException();
                case "Azure":
                    // Load from Azure Storage
                    throw new NotImplementedException();

            }

            return backgroundImageBytes;
        }

        // Get Overlay image from URL.
        // Future improvement, pass in the Overlay Image URL via the API params.
        public async Task<Image> GetOverlayImage()
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(_overlayImageUrl);
            byte[] overlayImageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return await Task.FromResult(Image.Load<Rgba32>(overlayImageBytes));
        }
    }
}
