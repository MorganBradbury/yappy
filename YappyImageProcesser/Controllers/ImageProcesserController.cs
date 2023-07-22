using Microsoft.AspNetCore.Mvc;
using YappyImageProcesser.Interfaces;
using YappyImageProcesser.Models;

namespace YappyImageProcesser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcesserController : ControllerBase
    {
        private readonly IImageManipulator _imagesManipulator;

        public ImageProcesserController(IImageManipulator imagesManipulator)
        {
            _imagesManipulator = imagesManipulator;
        }

        // GET: api/<ImageProcesserController>
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the Manipulated image created by the ImagesManipulator Helper.
            GeneratedImage manipulatedImage = _imagesManipulator.LoadImage();
            return File(
                manipulatedImage.FileContents,
                manipulatedImage.FileType
            );
        }
    }
}
