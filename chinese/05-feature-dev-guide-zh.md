# ADNC 以 Student 为例的完整开发流程


[GitHub 仓库地址](https://github.com/alphayu/adnc)
本文以 Student 实体为例，结合项目代码风格，系统性地阐述在 ADNC 项目中，从 Entity 定义到 Controller 层，实现完整增删改查（CRUD）功能的标准流程。内容涵盖 Entity、EntityConfig、DTO、Validator、Service、AutoMapper、Controller 等关键环节，旨在规范开发实践，提升代码一致性与可维护性。

---

## 1. Repository 层

### 1.1 定义 Entity

在 `Repository\Entities` 目录下新建 Student 实体：

1. 实体类必须直接或间接继承 `EfEntity`。
2. 若实体包含 `CreateBy`、`CreateByTime`、`ModifyBy`、`ModifyTime` 字段，则需继承 `EfFullAuditEntity`。
3. 若实体仅包含 `CreateBy`、`CreateByTime` 字段，则需继承 `EfBasicAuditEntity`。
4. 若实体不包含上述审计字段，则继承 `EfEntity`。
5. 若实体需要支持软删除，则实现 `ISoftDelete` 接口。
6. 若实体需要支持乐观锁（行并发控制），则实现 `IConcurrency` 接口。

```csharp
namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Student
/// </summary>
public class Student : EfBasicAuditEntity, ISoftDelete
{
    public static readonly int Name_MaxLength = 50;

    /// <summary>
    /// student name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }
}
```

并需在 `EntityInfo.cs` 中进行注册：

```csharp
modelBuilder.Entity<Student>().ToTable("sch_student");
```

---

### 1.2 定义 EntityConfig

在 `Repository\Entities\Config` 目录下新建 StudentConfig 实体配置类：

```csharp
namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class StudentConfig : AbstractEntityTypeConfiguration<Student>
{
    public override void Configure(EntityTypeBuilder<Student> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(Student.Name_MaxLength);
    }
}
```

### 1.3 数据库迁移

实体定义完成后，需执行数据库迁移操作：

```powershell
cd src
# 添加迁移
dotnet ef migrations add Student --project Repository\Adnc.Demo.Admin.Repository.csproj --startup-project Api\Adnc.Demo.Admin.Api.csproj
# 更新数据库
dotnet ef database update --project Repository\Adnc.Demo.Admin.Repository.csproj --startup-project Api\Adnc.Demo.Admin.Api.csproj
```

---

## 2. Application/Application.Contracts 层

### 2.1 配置 AutoMapper

在 `Application\AutoMapper\MapperProfile.cs` 文件中添加映射配置：

```csharp
CreateMap<Student, StudentDto>();
CreateMap<StudentCreationDto, Student>();
```

### 2.2 创建 DTOs

在 `Application\Contracts\Dtos\Student` 下新建 DTOs：

1. 若 Dto 包含 CreateBy、CreateByTime、ModifyBy、ModifyTime 字段，可继承 OutputFullAuditInfoDto。
2. 若 Dto 仅包含 CreateBy、CreateByTime 字段，可继承 OutputBaseAuditDto。
3. 若 Dto 不包含上述审计字段，可继承 InputDto 或 OutputDto。
4. 分页搜索 Dto 须继承 SearchPagedDto。

```csharp
namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

/// <summary>
/// Represents the payload used to create a student.
/// </summary>
public class StudentCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

}

/// <summary>
/// Represents the payload used to update a student.
/// </summary>
public class StudentUpdationDto : StudentCreationDto
{ }

namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

/// <summary>
/// Represents the paging and filtering criteria used to search student records.
/// </summary>
public class StudentSearchPagedDto : SearchPagedDto
{ }

/// <summary>
/// Represents a student.
/// </summary>
[Serializable]
public class StudentDto : OutputBaseAuditDto
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

}
```

---

### 2.3 编写 Validators

在 `Application\Contracts\Dtos\Student\Validators` 目录下新建校验器类：

```csharp
using Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="StudentCreationDto"/> instances.
/// </summary>
public class StudentCreationDtoValidator : AbstractValidator<StudentCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StudentCreationDtoValidator"/> class.
    /// </summary>
    public StudentCreationDtoValidator()
    { 
        RuleFor(x => x.Name).NotEmpty().MaximumLength(Student.Name_MaxLength);
    }
}

/// <summary>
/// Validates <see cref="StudentUpdationDto"/> instances.
/// </summary>
public class StudentUpdationDtoValidator : AbstractValidator<StudentUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StudentUpdationDtoValidator"/> class.
    /// </summary>
    public StudentUpdationDtoValidator()
    {
        Include(new StudentCreationDtoValidator());
    }
}
```

---

### 2.4 IStudentService 接口定义

在 `Application\Contracts\Interfaces` 下新建 IStudentService：

1. 接口必须继承 `IAppService`。
2. 所有写操作必须返回 `ServiceResult` 或 `ServiceResult<T>`。
3. 读操作建议返回 `Dto` 类型，亦可返回 `ServiceResult<T>`。
4. 如需处理事务，可添加 [UnitOfWork] 特性，或在实现类中注入 IUnitOfWork 手动开启事务。
5. 如需 CAP 处理分布式事务（发布事件），可添加 [UnitOfWork(Distributed = true)] 特性，或在实现类中注入 IUnitOfWork 手动开启事务。

```csharp
using Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines student management services.
/// </summary>
public interface IStudentService : IAppService
{
    /// <summary>
    /// Creates a student.
    /// </summary>
    /// <param name="input">The student to create.</param>
    /// <returns>The ID of the created student.</returns>
    [OperateLog(LogName = "Create student")]
    Task<ServiceResult<IdDto>> CreateAsync(StudentCreationDto input);

    /// <summary>
    /// Updates a student.
    /// </summary>
    /// <param name="id">The student ID.</param>
    /// <param name="input">The student changes.</param>
    /// <returns>A result indicating whether the student was updated.</returns>
    [OperateLog(LogName = "Update student")]
    Task<ServiceResult> UpdateAsync(long id, StudentUpdationDto input);

    /// <summary>
    /// Deletes one or more student records.
    /// </summary>
    /// <param name="ids">The student IDs.</param>
    /// <returns>A result indicating whether the records were deleted.</returns>
    [OperateLog(LogName = "Delete student")]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// Gets a student by ID.
    /// </summary>
    /// <param name="id">The student ID.</param>
    /// <returns>The requested student, or <c>null</c> if it does not exist.</returns>
    Task<StudentDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of student records.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of student records.</returns>
    Task<PageModelDto<StudentDto>> GetPagedAsync(StudentSearchPagedDto input);
}
```

### 2.5 实现IStudentService

在 `Application\Services` 下新建 StudentService：

1. Service 类必须继承 AbstractAppService。
2. 业务逻辑校验失败时，统一返回 Problem，不得抛出异常。

```csharp
using Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

namespace Adnc.Demo.Admin.Application.Services;

/// <inheritdoc cref="IStudentService"/>
public class StudentService(IEfRepository<Student> studentRepo)
    : AbstractAppService, IStudentService
{
    /// <inheritdoc />
    public async Task<ServiceResult<IdDto>> CreateAsync(StudentCreationDto input)
    {
        input.TrimStringFields();
        var nameExists = await studentRepo.AnyAsync(x => x.Name == input.Name);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This student name already exists");
        }
        var entity = Mapper.Map<Student>(input, IdGenerater.GetNextId());
        await studentRepo.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> UpdateAsync(long id, StudentUpdationDto input)
    {
        input.TrimStringFields();
        var entity = await studentRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return Problem(HttpStatusCode.NotFound, "This student does not exist");
        }

        var nameExists = await studentRepo.AnyAsync(x => x.Name == input.Name && x.Id != id);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This student name already exists");
        }
        var newEntity = Mapper.Map(input, entity);
        await studentRepo.UpdateAsync(newEntity);
        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        await studentRepo.ExecuteDeleteAsync(x => ids.Contains(x.Id));
        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<StudentDto?> GetAsync(long id)
    {
        var entity = await studentRepo.FetchAsync(x => x.Id == id);
        return entity is null ? null : Mapper.Map<StudentDto>(entity);
    }

    /// <inheritdoc />
    public async Task<PageModelDto<StudentDto>> GetPagedAsync(StudentSearchPagedDto input)
    {
        input.TrimStringFields();
        var whereExpr = ExpressionCreator
            .New<Student>()
            .AndIf(input.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{input.Keywords}%"));

        var total = await studentRepo.CountAsync(whereExpr);
        if (total == 0)
        {
            return new PageModelDto<StudentDto>(input);
        }

        var entities = await studentRepo
                                        .Where(whereExpr)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(input.SkipRows())
                                        .Take(input.PageSize)
                                        .ToListAsync();
        var dtos = Mapper.Map<List<StudentDto>>(entities);
        return new PageModelDto<StudentDto>(input, dtos, total);
    }
}
```

---

## 3. API层

### 3.1 权限常量定义

可在 `Api\Consts.cs` 文件中添加权限常量定义（可选）：

```csharp
/// <summary>
/// Defines permission codes for student management.
/// </summary>
public static class Student
{
    public const string Create = "student-create";
    public const string Update = "student-update";
    public const string Delete = "student-delete";
    public const string Search = "student-search";
    public const string Get = "student-get";
}
```

### 3.2 编写 Controller

在 `Api\Controllers` 目录下新建 Controller 类：

1. Controller 类必须继承 AdncControllerBase。
2. 每个方法可根据需要添加权限特性（可选）：

```csharp
using Adnc.Demo.Admin.Application.Contracts.Dtos.Student;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages students.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/students")]
[ApiController]
public class StudentController(IStudentService studentService) : AdncControllerBase
{
    /// <summary>
    /// Creates a student.
    /// </summary>
    /// <param name="input">The student to create.</param>
    /// <returns>The ID of the created student.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Student.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] StudentCreationDto input)
        => CreatedResult(await studentService.CreateAsync(input));

    /// <summary>
    /// Updates a student.
    /// </summary>
    /// <param name="id">The student ID.</param>
    /// <param name="input">The student changes.</param>
    /// <returns>A result indicating whether the student was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Student.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] StudentUpdationDto input)
        => Result(await studentService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more student records.
    /// </summary>
    /// <param name="ids">The comma-separated student IDs.</param>
    /// <returns>A result indicating whether the records were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Student.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await studentService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets a paged list of student records.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of student records.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Student.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<StudentDto>>> GetPagedAsync([FromQuery] StudentSearchPagedDto input)
        => await studentService.GetPagedAsync(input);

    /// <summary>
    /// Gets a student by ID.
    /// </summary>
    /// <param name="id">The student ID.</param>
    /// <returns>The requested student.</returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.Student.Get, PermissionConsts.Student.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> GetAsync([FromRoute] long id)
    {
        var dto = await studentService.GetAsync(id);
        return dto is null ? NotFound() : dto;
    }
}
```

---

## 4. 总结

综上所述，按照上述流程可在 ADNC 架构下规范、高效地实现 Student 实体的完整功能。开发过程中需注意：所有写操作在业务校验失败时统一返回 Problem，不抛出异常，确保业务异常处理一致、前后端交互友好，从而提升系统的可维护性与扩展性。
