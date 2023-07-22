using YappyImageProcesser.Interfaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using YappyImageProcesser.Models;

namespace YappyImageProcesser.Helpers
{
    public class ImageManipulator : IImageManipulator
    {
        // Height and Width for the overlay image
        private readonly int overlayImageHeight = 257;
        private readonly int overlayImageWidth = 257;

        // Position for the overlay
        private readonly int overlayPositionX = 315;
        private readonly int overlayPositionY = 266;

        // File extension to pass back to the controller when file has been generated
        private readonly string backgroundImageFileExtension = "image/jpg";

        // Instance of the File Processer. This returns the Images which are used for the manipulation. 
        private readonly IFileProcesser _fileProcesser;

        // Constructor
        public ImageManipulator(IFileProcesser imagesProcesser)
        {
            _fileProcesser = imagesProcesser;
        }

        // Method Loads the images required for the manipulation and returns the final image once generated.
        public GeneratedImage LoadImage()
        {
            Image backgroundImage = RetrieveBackgroundImage();
            Image overlayImage = RetrieveOverlayImage();
            overlayImage = ResizeImage(overlayImage);
            byte[] generateImage = GenerateImage(backgroundImage, overlayImage);
            GeneratedImage finalImage = new()
            {
                FileContents = generateImage,
                FileType = backgroundImageFileExtension
            };
            return finalImage;
        }

        // Methods below are private utility functions used for the LoadImage function.

        // Retrieves Images needed for the manipulation
        // IMPROVEMENT: For future improvement to this solution, I would pass in data for where the images need to be loaded from.
        private Image RetrieveBackgroundImage() => _fileProcesser.GetBackgroundImage("Local");
        private Image RetrieveOverlayImage() => _fileProcesser.GetOverlayImage().Result;

        // Generate image method. Pass in two images and this method will generate the manipulated image.
        private byte[] GenerateImage(Image backgroundImage, Image overlayImage)
        {
            // Generate Canvas for the images to be manipulated.
            using Image imageWithOverlay = new Image<Rgba32>(backgroundImage.Width, backgroundImage.Height);

            // Draw the background image on the new canvas
            imageWithOverlay.Mutate(ctx => ctx.DrawImage(backgroundImage, new Point(0, 0), 1f));

            // Draw the overlay image on the new canvas with the specified position
            imageWithOverlay.Mutate(ctx => ctx.DrawImage(overlayImage, new Point((int)overlayPositionX, (int)overlayPositionY), 1f));

            // Save the result to a byte array
            using MemoryStream outputStream = new MemoryStream();
            // Save the image to the memory stream
            imageWithOverlay.SaveAsJpeg(outputStream);
            // Return bytes[].
            return outputStream.ToArray();
        }

        // Generates the Size Object which is used to resize an image.
        private static Size ResizeMeasurements(int newImageWidth, int newImageHeight) => new(newImageWidth, newImageHeight);

        // Resizes the image based on the size provided
        private Image ResizeImage(Image imageToResize)
        {
            // Creates Size Object which is then used to resize the image below
            Size newSize = ResizeMeasurements(overlayImageWidth, overlayImageHeight);
            // Resizes the image
            imageToResize.Mutate(ctx => ctx.Resize(new ResizeOptions
            {
                Size = newSize,
                Mode = ResizeMode.BoxPad
            }));

            // Returns the resized image.
            return imageToResize;
        }
    }
}
