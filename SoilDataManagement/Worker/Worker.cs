namespace SoilDataManagement.Worker;

public class Worker : IWorker
{
    public async Task DoWork(CancellationToken cancelToken)
    {
        while (!cancelToken.IsCancellationRequested)
        {

        }
    }
}

