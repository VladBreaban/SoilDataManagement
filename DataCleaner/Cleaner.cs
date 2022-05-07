using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace DataCleaner;

public class Cleaner : IDataCleaner
{
    private readonly ILogger<Cleaner> _logger;
    private readonly IOptionsMonitor<DataCleanerOptionsMonitor> _dataCleanerOptionsMonitor;

    public Cleaner(ILogger<Cleaner> logger, IOptionsMonitor<DataCleanerOptionsMonitor> dataCleanerOptionsMonitor)
    {
        _logger = logger;
        _dataCleanerOptionsMonitor = dataCleanerOptionsMonitor;
    }
    ///<summary>
    ///Notice that, the sensor https://www.amazon.com/RS485-type-temperature-humidity-transmitter/dp/B07JKCDQM7
    ///Used for this project can measure data only up to the value of 255 for all of the three chemical components
    ///And, as a sensor, there may be erroneous measurements, with values over 255 that will be considered redundant data and we will eliminate themA
    ///</summary>
    public async Task<string> GetCleanData(string fileToBeCleanedPath)
    {
        string[] csvLines = await File.ReadAllLinesAsync(fileToBeCleanedPath);
        StringBuilder sb = new StringBuilder();
        int i = 0;
        int maxSensorValue = _dataCleanerOptionsMonitor.CurrentValue.MaxSensorValue;
        var path = Path.Combine(_dataCleanerOptionsMonitor.CurrentValue.DesiredCleanedFileLocation, DateTime.Now.ToString("yyyyMMdd") + "cleanData.csv");

        foreach (string csvLine in csvLines)
        {
            //first line contains columns title
            if(i==0)
            {
                string newLine = string.Join(",","CreatedDate","N","P","K");
                sb.Append(newLine + Environment.NewLine);
            }
            else
            {
                List<string> lineValues = new(csvLine.Split(','));
                if(lineValues.Count<5)
                {
                    _logger.LogError($"How i get less than 5 values on a row from thing speak? Strangee");
                }
                else if(Convert.ToInt32(lineValues.ElementAt(2)) <= maxSensorValue && Convert.ToInt32(lineValues.ElementAt(3)) <= maxSensorValue && Convert.ToInt32(lineValues.ElementAt(4)) <= maxSensorValue)
                {
                    //notice that we have the following position in the original csv:
                    // 0 --> created entry date
                    //2 --> N(nitrogen) measured value
                    //3-->P(phosphorus) measured value
                    //4-->K(potassium) measured value;
                    string newLine = string.Join(",", lineValues.ElementAt(0), lineValues.ElementAt(2), lineValues.ElementAt(3), lineValues.ElementAt(4));
                    sb.Append(newLine + Environment.NewLine);
                }
                else
                {
                    _logger.LogWarning($"Messy on line {i}");
                }                

            }
            i++;
        }
        await File.WriteAllTextAsync(path, sb.ToString());

        if(File.Exists(path))
        {
            File.Delete(fileToBeCleanedPath);
            return path;
        }
        else
        {
            throw new Exception($"Cleaned file does not exist. Possible path:{path} ");
        }


    }
}

