using Microsoft.Extensions.Options;
using SmartCity.Api.Configuration;

namespace SmartCity.Api.Services;

// Runs the RSS ingestion once on startup, then on a fixed interval (configured via
// RssIngestion:RefreshIntervalMinutes). Uses a scope per cycle since RssIngestionService
// depends on scoped services (ExternalNewsService/repository).
public class RssIngestionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RssIngestionSettings _settings;
    private readonly ILogger<RssIngestionBackgroundService> _logger;

    public RssIngestionBackgroundService(
        IServiceScopeFactory scopeFactory,
        IOptions<RssIngestionSettings> options,
        ILogger<RssIngestionBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = TimeSpan.FromMinutes(Math.Max(1, _settings.RefreshIntervalMinutes));

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunIngestionCycleAsync(stoppingToken);

            try
            {
                await Task.Delay(interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task RunIngestionCycleAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var ingestionService = scope.ServiceProvider.GetRequiredService<RssIngestionService>();

        try
        {
            var result = await ingestionService.IngestAllAsync(stoppingToken);
            _logger.LogInformation(
                "RSS ingestion cycle complete: fetched={Fetched} relevant={Relevant} inserted={Inserted} updated={Updated} skipped={Skipped} failed={Failed}",
                result.Fetched, result.Relevant, result.Inserted, result.Updated, result.Skipped, result.Failed);
        }
        catch (Exception ex)
        {
            // Never let an ingestion-cycle failure crash the background service.
            _logger.LogError(ex, "RSS ingestion cycle failed unexpectedly.");
        }
    }
}
