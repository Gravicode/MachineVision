using MachineVision.App.Services.Algorithm;
using MachineVision.App.Services.Picker;
using MachineVision.App.ViewModels.Base;

namespace MachineVision.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            ViewModelLocator.Instance.Register<IContourService, ContourService>();
            ViewModelLocator.Instance.Register<ICornerHarrisService, CornerHarrisService>();
            ViewModelLocator.Instance.Register<IDisparityService, DisparityService>();
            ViewModelLocator.Instance.Register<IFeatureDetectService, FeatureDetectService>();
            ViewModelLocator.Instance.Register<IFeatureMatchService, FeatureMatchService>();
            ViewModelLocator.Instance.Register<IPickerService, PickerService>();

            MainPage = new AppShell();
        }
    }
}