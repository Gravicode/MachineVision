//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

namespace MachineVision.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomNavigationView : NavigationPage
    {
        public CustomNavigationView() : base() => InitializeComponent();

        public CustomNavigationView(Page root) : base(root) => InitializeComponent();
    }
}