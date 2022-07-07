using System.Threading.Tasks;
using MachineVision.App.ViewModels.Base;

namespace MachineVision.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        MenuViewModel menuViewModel;

        public MainViewModel(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;
        }

        public override Task InitializeAsync(object navigationData) => Task.WhenAll
        (
            menuViewModel.InitializeAsync(navigationData),
            NavigationService.NavigateToAsync<HomeViewModel>()
        );

        public MenuViewModel MenuViewModel
        {
            get => menuViewModel;
            set
            {
                menuViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
