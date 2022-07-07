using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace MachineVision.App.Helpers
{
    public static class ImageHelper
    {

        public static System.Drawing.Bitmap FromByteArray(byte[] imageData)
        {
            var ms = new MemoryStream(imageData);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            return (System.Drawing.Bitmap)image;
        }
        public static Image<Bgr, byte> GetImage(string filename)
        {
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                return new Image<Bgr, byte>(filename);
            }

            return new Image<Bgr, byte>(640, 480);
        }

        public static byte[] SetImage(Image<Bgr, byte> resultImage)
        {
            if (resultImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    resultImage.ToBitmap().Save(memoryStream, ImageFormat.Bmp);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return memoryStream.ToArray();
                }
            }

            return null;
        }
    }
}
