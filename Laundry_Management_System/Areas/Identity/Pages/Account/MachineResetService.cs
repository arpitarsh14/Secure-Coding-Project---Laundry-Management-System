public class MachineResetService : IHostedService, IDisposable
{
    private readonly IServiceProvider _services;
    private readonly Timer _timer;

    public MachineResetService(IServiceProvider services)
    {
        _services = services;
        _timer = new Timer(ResetMachines, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Check every 1 minute
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private void ResetMachines(object state)
    {
        using (var scope = _services.CreateScope())
        {
            var machineService = scope.ServiceProvider.GetRequiredService<MachineService>();
            machineService.ResetMachinesIfExpired();
        }
    }
}
