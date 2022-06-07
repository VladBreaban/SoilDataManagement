namespace SoilDataManagement;

    public class ElasticDataSender : IHostedService
    {
        private ILogger<ElasticDataSender> _logger;
        private IWorker _worker;

        public ElasticDataSender(ILogger<ElasticDataSender> logger, IWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }

        public async Task StartAsync(CancellationToken cancelToken)
        {
        await _worker.DoWork(cancelToken);
        }

        public Task StopAsync(CancellationToken cancelToken)
        {
            _logger.LogWarning("ElasticDataSender service stopping...");
            return Task.CompletedTask;
        }
    }

