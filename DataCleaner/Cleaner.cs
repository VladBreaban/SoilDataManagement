
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
    ///

    public async Task<string> GenerateCleanDataFileForACertainField(string path, string field)
    {
        StringBuilder sb = new StringBuilder();
        var dataPredictionPath = Path.Combine(_dataCleanerOptionsMonitor.CurrentValue.DesiredCleanedFileLocation, DateTime.Now.ToString("yyyyMMdd") + "cleanData.csv");
        string newLineHeader = string.Join(",", "CreatedDate", "N");
        sb.Append(newLineHeader + Environment.NewLine);
        var allCleanedMeasuredData = await GetCleanData(path);
        if (allCleanedMeasuredData == null)
            return "";
    
        foreach(var measure in allCleanedMeasuredData)
        {
            string toBeConcatenate = "";
            switch (field)
            {
                case "N": toBeConcatenate = measure.N.ToString(); break;
                case "P": toBeConcatenate = measure.P.ToString(); break;
                case "K": toBeConcatenate =  measure.K.ToString(); break;
                default: toBeConcatenate = ""; break;
            }

            if(toBeConcatenate == "")
            {
                continue;
            }
            string newLine = string.Join(",", measure.CreatedDate, toBeConcatenate);
            sb.Append(newLine + Environment.NewLine);

        }
        await File.WriteAllTextAsync(dataPredictionPath, sb.ToString());

        return dataPredictionPath;

    }

    public async Task<List<MeasuredData>> GetCleanDataAverageValues(string fileToBeCleanedPath)
    {
        var cleanedData = await GetCleanData(fileToBeCleanedPath);
        List<MeasuredData> result = new List<MeasuredData>();
        var dates = cleanedData.Select(x=> x.CreatedDate.Date.ToString("yyyy-MM-dd")).Distinct().ToList();
        dates.ForEach(x =>
        {
            var obj = new MeasuredData()
            {
                CreatedDate = Convert.ToDateTime(x),
                N= cleanedData.Where(d => d.CreatedDate.Date.ToString("yyyy-MM-dd") == x).Select(x => x.N).Average(),
                P = cleanedData.Where(d => d.CreatedDate.Date.ToString("yyyy-MM-dd") == x).Select(x => x.P).Average(),
                K = cleanedData.Where(d => d.CreatedDate.Date.ToString("yyyy-MM-dd") == x).Select(x => x.K).Average()
            };
            result.Add(obj);
        });

        return result;
    }

    public async Task<List<MeasuredData>> GetCleanData(string fileToBeCleanedPath)
    {
        List<MeasuredData> data = new List<MeasuredData>(); 
        string[] csvLines = await File.ReadAllLinesAsync(fileToBeCleanedPath);
        StringBuilder sb = new StringBuilder();
        int i = 0;
        int maxSensorValue = _dataCleanerOptionsMonitor.CurrentValue.MaxSensorValue;

        foreach (string csvLine in csvLines)
        {
            //first line contains columns title
            if(i==0)
            {
                string newLine = string.Join(",","CreatedDate","N");
                sb.Append(newLine + Environment.NewLine);
            }
            else
            {
                List<string> lineValues = new(csvLine.Split(','));
                if(lineValues.Count<5)
                {
                    _logger.LogError($"How i get less than 5 values on a row from thing speak? Strangee");
                }
                //move 25 in appsettings ( it s the smalles value that can be measured by app and it s not messy)
                else if((Convert.ToInt32(lineValues.ElementAt(2)) <= maxSensorValue && (Convert.ToInt32(lineValues.ElementAt(2)) >= 25)) && (Convert.ToInt32(lineValues.ElementAt(3)) <= maxSensorValue && (Convert.ToInt32(lineValues.ElementAt(3)) >= 25)) && (Convert.ToInt32(lineValues.ElementAt(4)) <= maxSensorValue && (Convert.ToInt32(lineValues.ElementAt(2)) >= 25)))
                {
                    //notice that we have the following position in the original csv:
                    // 0 --> created entry date
                    //2 --> N(nitrogen) measured value
                    //3-->P(phosphorus) measured value
                    //4-->K(potassium) measured value;
                    string newLine = string.Join(",", lineValues.ElementAt(0), lineValues.ElementAt(2), lineValues.ElementAt(3), lineValues.ElementAt(4));
                    sb.Append(newLine + Environment.NewLine);
                    DateTime createdDate;
                    DateTime.TryParseExact(lineValues.ElementAt(0), "yyyy-MM-dd HH:mm:ss UTC", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDate);
                    data.Add(new MeasuredData
                    {
                        CreatedDate = createdDate,
                        N = float.Parse(lineValues.ElementAt(2)),
                        P = float.Parse(lineValues.ElementAt(3)),
                        K = float.Parse(lineValues.ElementAt(4))
                    });
                }
                else
                {
                    _logger.LogWarning($"Messy on line {i}");
                }                

            }
            i++;
        }

        return data;


    }
}

