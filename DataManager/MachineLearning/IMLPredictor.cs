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
        void TrainAndPredict(string cleanDataPath, string modelPath);
    }
}
