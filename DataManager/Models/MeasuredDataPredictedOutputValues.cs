using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Models
{
    public class MeasuredDataPredictedOutputValues
    {
        public float[] forecastedN { get; set; }

        public float[]  lowerBoundN { get; set; }

        public float[] upperBoundN { get; set; }
    }
}
