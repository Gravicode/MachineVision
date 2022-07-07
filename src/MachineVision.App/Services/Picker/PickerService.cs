using Microsoft.Win32;
using MachineVision.App.Services.Picker;

//[assembly: Xamarin.Forms.Dependency(typeof(PickerService))]
namespace MachineVision.App.Services.Picker
{
    public class PickerService : IPickerService
    {
        public async Task<string> PickImageFile()
        {
            try
            {
                PickOptions options = new PickOptions() { PickerTitle = "Pick a file" };
                var result = await FilePicker.Default.PickAsync(options);


                return result.FullPath;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }
    }
}
