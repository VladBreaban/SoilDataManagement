using DataManager.MachineLearning;
using Nest;
using RestSharp;
using SoilDataManagement;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var fact = NLogBuilder.ConfigureNLog("nlog.config");
var logger = fact.GetCurrentClassLogger();
builder.Host.UseNLog();
builder.WebHost.UseKestrel();
//builder.WebHost.UseUrls("http://*:7075");
//builder.WebHost.UseIIS();
builder.Services.AddControllers();
var elasticMetrictsUrl = builder.Configuration.GetValue<string>("MetricsElasticUri") ?? "http://lb-2kag5kqqav4cg.centralus.cloudapp.azure.com:9200/";

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddTransient<IDataManager, Manager>();
builder.Services.AddTransient<IDataCleaner, Cleaner>();
builder.Services.AddSingleton<IWorker, Worker>();
builder.Services.AddTransient<IElasticClient, Nest.ElasticClient>();
builder.Services.AddTransient<ElasticSearchClient>(x=> new ElasticSearchClient(elasticMetrictsUrl));
builder.Services.AddTransient<IElasticHelper, ElasticHelper>();
builder.Services.AddTransient<IMLPredictor, MLPredictor>();
builder.Services.Configure<ThingSpeakOptionsMonitor>(builder.Configuration.GetSection(nameof(ThingSpeakOptionsMonitor)));
builder.Services.Configure<DataCleanerOptionsMonitor>(builder.Configuration.GetSection(nameof(DataCleanerOptionsMonitor)));

builder.Services.AddHostedService<ElasticDataSender>();
builder.Host.UseWindowsService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
string localIP = LocalIPAddress();

app.Urls.Add("http://" + localIP + ":5077");
app.Urls.Add("https://" + localIP + ":7075");
static string LocalIPAddress()
{
    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
    {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
        if (endPoint != null)
        {
            return endPoint.Address.ToString();
        }
        else
        {
            return "127.0.0.1";
        }
    }
}
app.UseCors(c => c.SetIsOriginAllowed(origin => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
app.Run();
