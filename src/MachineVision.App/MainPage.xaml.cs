using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MachineVision.App.Helpers;

namespace MachineVision.App
{
    public partial class MainPage : ContentPage
    {
        public Mat matImage { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
        void ShowImage(System.Drawing.Bitmap bmp, string Desc = "")
        {
            var tempPath = Path.GetTempFileName() + ".jpg";
            bmp.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            Image1.Source = ImageSource.FromFile(tempPath);
            TxtKeterangan.Text = Desc;
        }
        private async void CircleHoughClicked(object sender, EventArgs e)
        {
            if (matImage != null)
            {
                var shapes = CVHelper.DetectCircleHough(matImage);               
                var bmp = shapes.ImageResult.ToBitmap();
                var desc = shapes.Description.Select(x => $"{x.shape} = {x.count}").ToArray();
                var keterangan = String.Join(", ", desc);
                ShowImage(bmp, keterangan);
            }
            else
            {
                await DisplayAlert("Info", "Pilih File dulu","Close");
            }
            /*
            Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //Draw "Hello, world." on the image using the specific font
            CvInvoke.PutText(
               img,
               "Hello, world",
               new System.Drawing.Point(10, 80),
               FontFace.HersheyComplex,
               1.0,
               new Bgr(0, 255, 0).MCvScalar);
            var bmp = img.ToBitmap();

            var tempPath = Path.GetTempFileName() + ".jpg";
            bmp.Save(tempPath,System.Drawing.Imaging.ImageFormat.Jpeg);
            Image1.Source = ImageSource.FromFile(tempPath);
            SemanticScreenReader.Announce(BtnProcess.Text);*/
        }
        public static byte[] ImageToByte2(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private async void BtnOpenFile_Clicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "jpg", "png", "gif" } }, // or general UTType values
                    { DevicePlatform.Android, new[] { "image/jpeg" , "image/png", "image/gif" } },
                    { DevicePlatform.WinUI, new[] { ".jpg", ".png", ".bmp", ".gif" } },
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "jpg", "png", "gif" } }, // or general UTType values
                });

            var result = await FilePickerHelper.PickAndShow(new PickOptions()
            {
                PickerTitle = "Select a file",
                FileTypes = customFileType

            });
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var bytes = ByteHelper.StreamToBytes(stream);
                    var bmp = ImageHelper.FromByteArray(bytes);
                    matImage = bmp.ToMat();
                    ShowImage(bmp,"Image Loaded..");
                }
            }
        }

        private async void CircleContourClicked(object sender, EventArgs e)
        {
            if (matImage != null)
            {
                var shapes = CVHelper.DetectCircleContour(matImage);
                var bmp = shapes.ImageResult.ToBitmap();
                var desc = shapes.Description.Select(x => $"{x.shape} = {x.count}").ToArray();
                var keterangan = String.Join(", ", desc);
                ShowImage(bmp, keterangan);
            }
            else
            {
                await DisplayAlert("Info", "Pilih File dulu", "Close");
            }
        }
    }
}