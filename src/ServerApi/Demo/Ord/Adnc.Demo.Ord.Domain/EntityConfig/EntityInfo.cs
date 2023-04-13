using Adnc.Shared;

namespace Adnc.Demo.Ord.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    public EntityInfo(UserContext userContext) : base(userContext)
    {
    }
    
    protected override Assembly GetCurrentAssembly() => GetType().Assembly;
}