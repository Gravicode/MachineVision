using MachineVision.App.Models.Algorithm;

namespace MachineVision.App.Services.Algorithm
{
    public interface IFeatureMatchService
    {
        AlgorithmResult DetectFeatureMatch(
            string modelName,
            string observedeName,
            FeatureDetectType detectType,
            FeatureMatchType matchType,
            int k,
            double uniquenessThreshold);
    }
}
