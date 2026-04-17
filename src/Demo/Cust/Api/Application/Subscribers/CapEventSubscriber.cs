using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Cust.Api.Application.Subscribers;

public sealed partial class CapEventSubscriber(IUnitOfWork unitOfWork, IEfRepository<Finance> finaceRepo, IEfRepository<TransactionLog> transactionLogRepo
    , ILogger<CapEventSubscriber> logger, MessageTrackerFactory trackerFactory) : ICapSubscribe
{
    private readonly IMessageTracker _tracker = trackerFactory.Create();

    /// <summary>
    /// 订阅充值事件
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(CustomerRechargedEvent))]
    public async Task HandleCustomerRechargedEvent(CustomerRechargedEvent eventDto)
    {
        eventDto.TrimStringFields();

        var eventId = eventDto.Id;
        var eventHandler = nameof(HandleCustomerRechargedEvent);
        var hasProcessed = await _tracker.HasProcessedAsync(eventId, eventHandler);
        if (hasProcessed)
        {
            return;
        }

        var transLog = await transactionLogRepo.FetchAsync(x => x.Id == eventDto.TransactionLogId, noTracking: false);
        if (transLog is null)
        {
            return;
        }

        var finance = await finaceRepo.FetchAsync(x => x.Id == eventDto.CustomerId, noTracking: false);
        if (finance is null)
        {
            return;
        }

        try
        {
            unitOfWork.BeginTransaction();

            var originalBalance = finance.Balance;
            var changedBalance = originalBalance + eventDto.Amount;
            finance.Balance = changedBalance;
            await finaceRepo.UpdateAsync(finance);

            transLog.ExchageStatus = ExchageStatus.Finished;
            transLog.ChangingAmount = originalBalance;
            transLog.ChangedAmount = changedBalance;
            await transactionLogRepo.UpdateAsync(transLog);

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

    /// <summary>
    /// 订阅付款事件
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

            logger.LogInformation("------开始处理[{eventId}]------", eventId);
            //TODO
            //
            //
            logger.LogInformation("------完成处理[{eventId}]------", eventId);

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
