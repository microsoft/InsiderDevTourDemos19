using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorInsider.App.Services;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrdersHandler;

namespace BlazorInsider.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DatabaseService _databaseService;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _databaseService = new DatabaseService();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Checking orders at {DateTimeOffset.Now}");
                try
                {

                    Channel channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
                    OrdersManager.OrdersManagerClient client = new OrdersManager.OrdersManagerClient(channel);

                    OrderRequest request = new OrderRequest();
                    OrderReply result = await client.GetNewOrderAsync(request);
                    if (result.OrderId != 0)
                    {
                        _databaseService.UpdateOrder(result.OrderId);
                        _logger.LogInformation($"Order with id {result.OrderId} has been processed");
                    }
                    else
                    {
                        _logger.LogInformation($"No pending orders at {DateTimeOffset.Now}");
                    }

                    await channel.ShutdownAsync();
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message + exc.StackTrace);
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
