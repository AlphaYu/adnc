using Adnc.Shared;
using Adnc.Shared.Repository.EfEntities;

namespace Adnc.UnitTest.TestCases.Repositories.Entities;

public class EntityInfo : AbstracSharedEntityInfo
{
    public EntityInfo(UserContext userContext) : base(userContext)
    {
    }

    protected override Assembly GetCurrentAssembly() => GetType().Assembly;
}