//using Xamarin.Forms;

namespace MachineVision.App.Models.Algorithm
{
    public class AlgorithmModel : BindableObject
    {
        FeatureDetectType algorithmType;
        string name;

        public FeatureDetectType AlgorithmType
        {
            get => algorithmType;
            set
            {
                algorithmType = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
}
