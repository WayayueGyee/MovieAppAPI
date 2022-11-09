using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace MovieAppAPI.Helpers;

public static class GuidExtension {
    public static bool IsGreaterThan(this Guid left, Guid right) {
        return left.CompareTo(right) > 0;
    }

    public static bool IsGreaterThanOrEqual(this Guid left, Guid right) {
        return left.CompareTo(right) >= 0;
    }

    public static bool IsLessThan(this Guid left, Guid right) {
        return left.CompareTo(right) < 0;
    }

    public static bool IsLessThanOrEqual(this Guid left, Guid right) {
        return left.CompareTo(right) <= 0;
    }

    public static void Register(ModelBuilder modelBuilder) {
        RegisterFunction(modelBuilder, nameof(IsGreaterThan), ExpressionType.GreaterThan);
        RegisterFunction(modelBuilder, nameof(IsGreaterThanOrEqual), ExpressionType.GreaterThanOrEqual);
        RegisterFunction(modelBuilder, nameof(IsLessThan), ExpressionType.LessThan);
        RegisterFunction(modelBuilder, nameof(IsLessThanOrEqual), ExpressionType.LessThanOrEqual);
    }

    private static void RegisterFunction(ModelBuilder modelBuilder, string name, ExpressionType type) {
        var method = typeof(GuidExtension).GetMethod(name, new[] { typeof(Guid), typeof(Guid) });
        if (method != null)
            modelBuilder.HasDbFunction(method).HasTranslation(parameters => {
                var left = parameters.ElementAt(0);
                var right = parameters.ElementAt(1);

                return new SqlBinaryExpression(type, left, right, typeof(bool), null);
            });
    }
}