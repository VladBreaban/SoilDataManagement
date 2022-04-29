
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var fact = NLogBuilder.ConfigureNLog("nlog.config");
var logger = fact.GetCurrentClassLogger();
builder.Host.UseNLog();
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://*:7075");
builder.WebHost.UseIIS();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddTransient<IDataManager, DataManager>();
builder.Services.AddTransient<IWorker, Worker>();
builder.Services.Configure<ThingSpeakOptionsMonitor>(builder.Configuration.GetSection(nameof(ThingSpeakOptionsMonitor)));

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
