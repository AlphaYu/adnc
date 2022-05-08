namespace Adnc.Infra.EfCore.Transaction;

public class UnitOfWorkStatus
{
    public bool IsStartingUow { get; internal set; }
}