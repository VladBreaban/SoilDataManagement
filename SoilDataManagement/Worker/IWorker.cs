namespace SoilDataManagement.Worker;

public interface IWorker
{
    Task DoWork(CancellationToken cancelToken);
}

