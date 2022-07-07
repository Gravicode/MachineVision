using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MachineVision.App.Models.Algorithm;
using MachineVision.App.Services.Algorithm;
using MachineVision.App.Helpers;
using MachineVision.App.Services.Algorithm;
using Color = System.Drawing.Color;
//[assembly: Xamarin.Forms.Dependency(typeof(CornerHarrisService))]
namespace MachineVision.App.Services.Algorithm
{
    public class CornerHarrisService : ICornerHarrisService
    {
        public AlgorithmResult DetectCornerHarris(
            string filename,
            byte threshold,
            int blockSize,
            int apertureSize,
            double k,
            HarrisBorderType borderType)
        {
            AlgorithmResult result = new AlgorithmResult();
            Image<Bgr, byte> image = ImageHelper.GetImage(filename);
            var resultImage = new Image<Bgr, byte>(filename);

            // Create new (gray, float) image for corner
            var corners = new Image<Gray, float>(image.Size);

            // Harris corner with GrayScale
            CvInvoke.CornerHarris(
                image.Convert<Gray, byte>(),
                corners,
                blockSize,
                apertureSize,
                k,
                GetBorderType(borderType));

            // Normalize
            CvInvoke.Normalize(corners, corners);

            // Set resultImage after normalizing
            resultImage = corners.Convert<Bgr, byte>();

            // Crate new (gray, byte) image for corner
            var gray = corners.Convert<Gray, byte>();
            result.CircleDatas = new List<CirclePointModel>();

            // for each pixel annotate the corner
            // if inensity is beyond the threshold
            for (var j = 0; j < gray.Rows; j++)
            {
                for (var i = 0; i < gray.Cols; i++)
                {
                    if (!(gray[j, i].Intensity > threshold))
                    {
                        continue;
                    }

                    var circle = new CircleF(new System.Drawing.PointF(i, j), 1);
                    resultImage.Draw(circle, new Bgr(Color.FromArgb(255, 77, 77)), 3);
                    result.CircleDatas.Add(new CirclePointModel()
                    {
                        CenterX = circle.Center.X,
                        CenterY = circle.Center.Y,
                        Radius = circle.Radius,
                        Area = circle.Area
                    });
                }
            }

            result.ImageArray = ImageHelper.SetImage(resultImage);

            return result;
        }

        BorderType GetBorderType(HarrisBorderType type)
        {
            switch (type)
            {
                case HarrisBorderType.Constant:
                    return BorderType.Constant;
                case HarrisBorderType.Replicate:
                    return BorderType.Replicate;
                case HarrisBorderType.Reflect:
                    return BorderType.Reflect;
                case HarrisBorderType.Wrap:
                    return BorderType.Wrap;
                case HarrisBorderType.Reflect101:
                    return BorderType.Reflect101;
                case HarrisBorderType.Transparent:
                    return BorderType.Transparent;
                case HarrisBorderType.Isolated:
                    return BorderType.Isolated;
                default:
                    return BorderType.Default;
            }
        }
    }
}
