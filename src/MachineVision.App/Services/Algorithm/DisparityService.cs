using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MachineVision.App.Models.Algorithm;
using MachineVision.App.Services.Algorithm;
using MachineVision.App.Helpers;
using MachineVision.App.Services.Algorithm;

//[assembly: Xamarin.Forms.Dependency(typeof(DisparityService))]
namespace MachineVision.App.Services.Algorithm
{
    public class DisparityService : IDisparityService
    {
        public AlgorithmResult DetectDisparity(
            string filenameL,
            string filenameR,
            int numberOfDisparities,
            int blockSize)
        {
            AlgorithmResult result = new AlgorithmResult();
            Image<Bgr, byte> imageLeft = ImageHelper.GetImage(filenameL);
            Image<Bgr, byte> imageRight = ImageHelper.GetImage(filenameR);
            var resultImage = new Image<Bgr, byte>(imageLeft.Width, imageLeft.Height);

            // Create new (gray, float) image for disparity
            var imageDisparity = new Image<Gray, float>(imageLeft.Size);

            StereoBM stereoBM = new StereoBM(numberOfDisparities, blockSize);
            stereoBM.Compute(
              
                imageLeft.Convert<Gray, byte>(),
                imageRight.Convert<Gray, byte>(),
                imageDisparity);

            // Normalize
            CvInvoke.Normalize(imageDisparity, imageDisparity, 0, 255, NormType.MinMax, DepthType.Cv8U);

            // Set resultImage after normalizing
            resultImage = imageDisparity.Convert<Bgr, byte>();

            result.ImageArray = ImageHelper.SetImage(resultImage);

            return result;
        }
    }
}
