using Adnc.Demo.Cust.Api.Repository.Entities;

namespace Adnc.Demo.Cust.Api.Application.Subscribers;

public sealed partial class CapEventSubscriber : ICapSubscribe
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEfRepository<Customer> _customerRepo;
    private readonly IEfRepository<CustomerFinance> _custFinaceRepo;
    private readonly IEfRepository<CustomerTransactionLog> _custTransactionLogRepo;
    private readonly ILogger<CapEventSubscriber> _logger;
    private readonly IMessageTracker _tracker;

    public CapEventSubscriber(
        IUnitOfWork unitOfWork,
        IEfRepository<Customer> customerRepo,
        IEfRepository<CustomerFinance> custFinaceRepo,
        IEfRepository<CustomerTransactionLog> custTransactionLogRepo,
        ILogger<CapEventSubscriber> logger,
        MessageTrackerFactory trackerFactory)
    {
        _unitOfWork = unitOfWork;
        _customerRepo = customerRepo;
        _custFinaceRepo = custFinaceRepo;
        _custTransactionLogRepo = custTransactionLogRepo;
        _logger = logger;
        _tracker = trackerFactory.Create();
    }

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
        var eventHandler = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty;
        var hasProcessed = await _tracker.HasProcessedAsync(eventId, eventHandler);
        if (hasProcessed)
            return;

        var transLog = await _custTransactionLogRepo.FindAsync(eventDto.TransactionLogId, noTracking: false);
        if (transLog is null)
            return;

        var finance = await _custFinaceRepo.FindAsync(eventDto.CustomerId, noTracking: false);
        if (finance is null)
            return;

        _unitOfWork.BeginTransaction();

        try
        {
            var originalBalance = finance.Balance;
            var changedBalance = originalBalance + eventDto.Amount;
            finance.Balance = changedBalance;
            await _custFinaceRepo.UpdateAsync(finance);

            transLog.ExchageStatus = ExchageStatus.Finished;
            transLog.ChangingAmount = originalBalance;
            transLog.ChangedAmount = changedBalance;
            await _custTransactionLogRepo.UpdateAsync(transLog);

            await _tracker.MarkAsProcessedAsync(eventId, eventHandler);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception($"{eventHandler}-{eventId}", ex);
        }
        finally
        {
            _unitOfWork.Dispose();
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
        var eventHandler = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty;
        var hasProcessed = await _tracker.HasProcessedAsync(eventId, eventHandler);
        if (hasProcessed)
            return;

        _unitOfWork.BeginTransaction();

        try
        {
            //TODO
            //
            //
            await _tracker.MarkAsProcessedAsync(eventId, eventHandler);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception($"{eventHandler}-{eventId}", ex);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}