
namespace DataManager.MachineLearning
{
    public interface IMLPredictor
    {
        public void TrainAndPredict(string cleanDataPath, string modelPath);
    }
}
