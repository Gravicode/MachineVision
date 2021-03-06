using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using MachineVision.App.Models.Algorithm;
using MachineVision.App.Services.Algorithm;
using MachineVision.App.Helpers;
using static Emgu.CV.Features2D.Features2DToolbox;
using static Emgu.CV.Features2D.KAZE;
using Color = System.Drawing.Color;
//[assembly: Xamarin.Forms.Dependency(typeof(FeatureDetectService))]
namespace MachineVision.App.Services.Algorithm
{
    public class FeatureDetectService : IFeatureDetectService
    {
        public AlgorithmResult DetectKaze(
            string filename,
            KeypointType kpsType,
            float threshold,
            int octaves,
            int sublevels)
        {
            AlgorithmResult result = new AlgorithmResult();
            Image<Bgr, byte> image = ImageHelper.GetImage(filename);
            Image<Bgr, byte> resultImage = new Image<Bgr, byte>(filename);

            // Get features from image
            var kaze = new KAZE(false, false, threshold, octaves, sublevels, Diffusivity.PmG2);
            var keyPoints = kaze.Detect(image);
            DrawKeypoints(
                image,
                new VectorOfKeyPoint(keyPoints),
                resultImage,
                new Bgr(Color.FromArgb(255, 77, 77)),
                GetKeypointDraw(kpsType));

            result.ImageArray = ImageHelper.SetImage(resultImage);
            result.KeyDatas = new List<KeyPointModel>();
            result.KeyDatas.AddRange(keyPoints.Select(k => new KeyPointModel()
            {
                X = k.Point.X,
                Y = k.Point.Y,
                Size = k.Size,
                Angle = k.Angle,
                Response = k.Response,
                Octave = k.Octave,
                ClassId = k.ClassId
            }));

            return result;
        }

        public AlgorithmResult DetectSift(
            string filename,
            KeypointType kpsType,
            int features,
            int octaveLayers,
            double contrastThreshold,
            double edgeThreshold,
            double sigma)
        {
            AlgorithmResult result = new AlgorithmResult();
            Image<Bgr, byte> image = ImageHelper.GetImage(filename);
            Image<Bgr, byte> resultImage = new Image<Bgr, byte>(filename);
            // Get features from image
            var sift = new SIFT(features, octaveLayers, contrastThreshold, edgeThreshold, sigma);
            var keyPoints = sift.Detect(image);
            DrawKeypoints(
                image,
                new VectorOfKeyPoint(keyPoints),
                resultImage,
                new Bgr(Color.FromArgb(255, 77, 77)),
                GetKeypointDraw(kpsType));

            result.ImageArray = ImageHelper.SetImage(resultImage);
            result.KeyDatas = new List<KeyPointModel>();
            result.KeyDatas.AddRange(keyPoints.Select(k => new KeyPointModel()
            {
                X = k.Point.X,
                Y = k.Point.Y,
                Size = k.Size,
                Angle = k.Angle,
                Response = k.Response,
                Octave = k.Octave,
                ClassId = k.ClassId
            }));
            return result;
        }

        public AlgorithmResult DetectSurf(
            string filename,
            KeypointType kpsType,
            double hessianThresh,
            int octaves,
            int octaveLayers)
        {
            throw new Exception("not supported in this emgu");
            //AlgorithmResult result = new AlgorithmResult();
            //Image<Bgr, byte> image = ImageHelper.GetImage(filename);
            //Image<Bgr, byte> resultImage = new Image<Bgr, byte>(filename);

            //// Get features from image
            //var surf = new SURF(hessianThresh, octaves, octaveLayers);
            //var keyPoints = surf.Detect(image);
            //DrawKeypoints(
            //    image,
            //    new VectorOfKeyPoint(keyPoints),
            //    resultImage,
            //    new Bgr(Color.FromArgb(255, 77, 77)),
            //    GetKeypointDraw(kpsType));

            //result.ImageArray = ImageHelper.SetImage(resultImage);
            //result.KeyDatas = new List<KeyPointModel>();
            //result.KeyDatas.AddRange(keyPoints.Select(k => new KeyPointModel()
            //{
            //    X = k.Point.X,
            //    Y = k.Point.Y,
            //    Size = k.Size,
            //    Angle = k.Angle,
            //    Response = k.Response,
            //    Octave = k.Octave,
            //    ClassId = k.ClassId
            //}));
            //return result;
        }

        KeypointDrawType GetKeypointDraw(KeypointType type)
        {
            switch (type)
            {
                case KeypointType.DrawRichKeypoints:
                    return KeypointDrawType.DrawRichKeypoints;
                case KeypointType.NotDrawSinglePoints:
                    return KeypointDrawType.NotDrawSinglePoints;
                default:
                    return KeypointDrawType.Default;
            }
        }
    }
}
