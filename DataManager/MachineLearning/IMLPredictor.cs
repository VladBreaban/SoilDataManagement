using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.MachineLearning
{
    public interface IMLPredictor
    {
        void TrainAndPredict(MLContext mlContext, string cleanDataPath, string modelPath);
    }
}
