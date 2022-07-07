using System.Threading.Tasks;
using MachineVision.App.Services.Navigation;

namespace MachineVision.App.ViewModels.Base
{
    public class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        protected readonly INavigationService NavigationService;

        public ViewModelBase() => NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();

        public virtual Task InitializeAsync(object navigationData) => Task.FromResult(false);
    }
}
