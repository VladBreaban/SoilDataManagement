namespace SoilDataManagement;

    public class DataSender : IHostedService
    {
        private ILogger<DataSender> _logger;
        private IWorker _worker;

        public DataSender(ILogger<DataSender> logger, IWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }

        public async Task StartAsync(CancellationToken cancelToken)
        {
        _ = _worker.DoWork(cancelToken);
        }

        public Task StopAsync(CancellationToken cancelToken)
        {
            _logger.LogWarning("ElasticDataSender service stopping...");
            return Task.CompletedTask;
        }
    }

