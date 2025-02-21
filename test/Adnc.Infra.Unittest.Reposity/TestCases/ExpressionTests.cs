using Adnc.Infra.Repository.EfCore.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace Adnc.Infra.Unittest.Reposity.TestCases;


public class ExpressionConverterTests
{
    private class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public double Salary { get; set; }
        public decimal Bonus { get; set; }
        public float TaxRate { get; set; }
    }


    [Fact]
    public void TransformExpression_ShouldCallApplyMethod()
    {
        // Arrange
        Expression<Func<MyEntity, MyEntity>> expr = x => new MyEntity
        {
            Name = x.Name + " Updated",
            Age = x.Age + 1
        };

        // Act
        var transformedExpr = ExpressionHelper.Transform(expr);

        // Assert
        Assert.NotNull(transformedExpr);

        // 获取表达式树，检查是否调用了 Apply 方法
        var body = transformedExpr.Body as MethodCallExpression;
        Assert.NotNull(body);
        Assert.Equal("Apply", body.Method.Name);

        // 获取 Apply 方法的参数，应该是 SetProperty 调用的链式表达式
        var firstCall = body.Arguments[0] as MethodCallExpression;
        Assert.NotNull(firstCall);
        Assert.Equal("SetProperty", firstCall.Method.Name);
    }
}
