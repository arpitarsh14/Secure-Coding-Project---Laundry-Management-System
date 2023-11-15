using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MachineAvailabilityService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public MachineAvailabilityService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _provider.CreateScope())
            {
                // Resolve your service to reset machine availability
                var machineService = scope.ServiceProvider.GetRequiredService<MachineService>();
                machineService.ResetMachineAvailabilityIfTimeoutReached();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }
    }
}
