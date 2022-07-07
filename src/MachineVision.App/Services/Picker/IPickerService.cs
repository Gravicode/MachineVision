namespace MachineVision.App.Services.Picker
{
    public interface IPickerService
    {
        Task<string> PickImageFile();
    }
}
