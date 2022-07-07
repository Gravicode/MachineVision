using MachineVision.App.Models.Algorithm;

namespace MachineVision.App.Services.Algorithm
{
    public interface IContourService
    {
        AlgorithmResult DetectContours(
            string filename,
            ContourRetrType rtype,
            ContourMethodType mtype,
            double threshold1,
            double threshold2,
            int apertureSize);
    }
}
