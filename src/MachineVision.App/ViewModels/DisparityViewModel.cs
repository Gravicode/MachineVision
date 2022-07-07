using System.IO;
using System.Windows.Input;
using MachineVision.App.Models.Algorithm;
using MachineVision.App.Services.Algorithm;
using MachineVision.App.Services.Picker;
using MachineVision.App.ViewModels.Base;
//using Xamarin.Forms;

namespace MachineVision.App.ViewModels
{
    public class DisparityViewModel : ViewModelBase
    {
        readonly IDisparityService disparityService;
        readonly IPickerService pickerService;
        ImageSource resultImage;
        string filenameL;
        string filenameR;
        bool isDone = false;

        public ICommand DetectCommand => new Command(DetectDisparity);
        public ICommand PickLeftCommand => new Command(PickImageL);
        public ICommand PickRightCommand => new Command(PickImageR);

        public DisparityViewModel()
        {
            disparityService = DependencyService.Get<IDisparityService>();
            pickerService = DependencyService.Get<IPickerService>();
        }

        public ImageSource ResultImage
        {
            get => resultImage;
            set => SetProperty(ref resultImage, value);
        }

        public string FileNameL
        {
            get => filenameL;
            set => SetProperty(ref filenameL, value);
        }

        public string FileNameR
        {
            get => filenameR;
            set => SetProperty(ref filenameR, value);
        }

        public bool IsDone
        {
            get => isDone;
            set => SetProperty(ref isDone, value);
        }

        #region Disparity

        int numberOfDisparities = 0;
        int blockSize = 21;

        public int NumberOfDisparities
        {
            get => numberOfDisparities;
            set => SetProperty(ref numberOfDisparities, value);
        }

        public int BlockSize
        {
            get => blockSize;
            set => SetProperty(ref blockSize, value);
        }

        #endregion

        void DetectDisparity()
        {
            IsBusy = true;

            // TODO: Add Points
            AlgorithmResult result = disparityService.DetectDisparity(
                FileNameL,
                FileNameR,
                NumberOfDisparities,
                BlockSize);

            ResultImage = ImageSource.FromStream(() => new MemoryStream(result.ImageArray));

            IsBusy = false;
            IsDone = true;
        }

        async void PickImageL()
        {
            IsDone = false;

            FileNameL = await pickerService.PickImageFile();
        }

        async void PickImageR()
        {
            IsDone = false;

            FileNameR = await pickerService.PickImageFile();
        }
    }
}
