using Castle.Core.Logging;
using Dias.ExcelSteam.Conversions;
using Dias.ExcelSteam.DataValidator;
using Dias.ExcelSteam.Dtos;
using Dias.ExcelSteam.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dias.ExcelSteam.Services
{
    public class MessageConsumer(
        IMetadataService metadataReaderservice,
        IMessageHandler messageHandler,
        IDataValidator<MaterialDto> dataValidator,
        IBulkInsertMaterial bulkInsert,
         ILogger<MessageConsumer> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = await metadataReaderservice.GetExcelReaderMetadataAsync().ConfigureAwait(false);
                    if (!result.Any())
                    {
                        logger.LogInformation("No messages to process. Waiting for new messages...");
                        await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                        continue;
                    }
                    foreach (var item in result)
                    {
                        try
                        {
                            logger.LogInformation($"Processing message for item ID {item.Id}.");
                            var data = await messageHandler.DeserializeMessage(item.Message).ConfigureAwait(false);
                            if (data != null)
                            {
                                bool isValid = await dataValidator.ValidateAsync(item.Id, data).ConfigureAwait(false);
                                if (!isValid)
                                {
                                    logger.LogWarning($"Validation failed for item ID {item.Id}. Skipping...");
                                    continue; // Skip to next item if validation fails
                                }

                                await bulkInsert.InsertData(item.Id, data, item.CreatedBy).ConfigureAwait(false);
                                logger.LogInformation($"Successfully inserted data for item ID {item.Id}.");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error deserializing message for item ID {item.Id}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing messages.");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

        }
    }
}
