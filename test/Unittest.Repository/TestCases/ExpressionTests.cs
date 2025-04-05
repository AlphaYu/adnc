using Adnc.Infra.Repository.EfCore.Internal;

namespace Adnc.Infra.Unittest.Reposity.TestCases;


public class ExpressionConverterTests
{

    [Fact]
    public void TransformExpression_ShouldConvertExpressionCorrectly()
    {
        // Arrange
        Expression<Func<MyEntity, MyEntity>> source = entity => new MyEntity
        {
            Name = entity.Name + " Updated",
            Age = entity.Age + 1
        };

        // Act
        var transformedExpr = ExpressionHelper.ConvertToSetPropertyCalls<MyEntity>(source);

        // Assert
        Assert.NotNull(transformedExpr);  // The transformed expression should not be null

        // Check that the body of the transformed expression is a method call to SetProperty
        var body = transformedExpr.Body as MethodCallExpression;
        Assert.NotNull(body);  // The body should be a method call

        // Verify that SetProperty method is called
        Assert.Equal("SetProperty", body.Method.Name);
    }
}

public class MyEntity : IFullAuditInfo
{
    public string Name { get; set; }
    public int Age { get; set; }
    public long ModifyBy { get; set; }
    public DateTime ModifyTime { get; set; }
    public long CreateBy { get; set; }
    public DateTime CreateTime { get; set; }
}

