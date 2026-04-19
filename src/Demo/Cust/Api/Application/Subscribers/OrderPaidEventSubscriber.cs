using Adnc.Demo.Remote.Event;
using Adnc.Infra.EventBus.Tracker;

namespace Adnc.Demo.Cust.Api.Application.Subscribers;

public sealed partial class OrderPaidEventSubscriber(IUnitOfWork unitOfWork, ILogger<OrderPaidEventSubscriber> logger, MessageTrackerFactory trackerFactory) : ICapSubscribe
{
    private readonly IMessageTracker _tracker = trackerFactory.Create();

    /// <summary>
    /// Subscribe to payment events
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderPaidEvent))]
    public async Task HandleOrderPaidEvent(OrderPaidEvent eventDto)
    {
        eventDto.TrimStringFields();

        var eventId = eventDto.Id;
        var eventHandler = nameof(HandleOrderPaidEvent);
        var hasProcessed = await _tracker.HasProcessedAsync(eventId, eventHandler);
        if (hasProcessed)
        {
            return;
        }

        try
        {
            unitOfWork.BeginTransaction();

            logger.LogInformation("------Start processing [{eventId}]------", eventId);
            //TODO
            //
            //
            logger.LogInformation("------Completed processing [{eventId}]------", eventId);

            await _tracker.MarkAsProcessedAsync(eventId, eventHandler);

            await unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            throw new InvalidOperationException($"{eventHandler}-{eventId}", ex);
        }
        finally
        {
            unitOfWork.Dispose();
        }
    }
}
