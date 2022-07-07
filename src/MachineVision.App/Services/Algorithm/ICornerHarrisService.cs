using MachineVision.App.Models.Algorithm;

namespace MachineVision.App.Services.Algorithm
{
    public interface ICornerHarrisService
    {
        AlgorithmResult DetectCornerHarris(
            string filename,
            byte threshold,
            int blockSize,
            int apertureSize,
            double k,
            HarrisBorderType borderType);
    }
}
