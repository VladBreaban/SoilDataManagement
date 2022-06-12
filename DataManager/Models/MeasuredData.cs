using Microsoft.ML.Data;

namespace DataManager.Models;

public class MeasuredData
{
    [LoadColumn(1)]
    public float N { get; set; }
    [LoadColumn(2)]
    public float P { get; set; }
    [LoadColumn(3)]
    public float K { get; set; }
    [LoadColumn(0)]
    public DateTime CreatedDate { get; set; }
    [LoadColumn(4)]
    public float year { get; set; }

    }

