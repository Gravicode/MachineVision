using MachineVision.App.Models.Algorithm;

namespace MachineVision.App.Services.Algorithm
{
    public interface IDisparityService
    {
        AlgorithmResult DetectDisparity(
            string filenameL,
            string filenameR,
            int numberOfDisparities,
            int blockSize);
    }
}
