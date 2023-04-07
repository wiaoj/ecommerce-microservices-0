using System.Linq.Expressions;

namespace BuildingBlocks.Core.Linq;
public static class PredicateBuilder {
	public static Expression<Func<Type, Boolean>> Build<Type>(String propertyName, String comparison, String value) {
		const String parameterName = "x";
		ParameterExpression parameter = Expression.Parameter(typeof(Type), parameterName);
		Expression left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
		Expression body = MakeComparison(left, comparison, value);
		return Expression.Lambda<Func<Type, Boolean>>(body, parameter);
	}

	public static Expression<Func<Type, Boolean>> And<Type>(this Expression<Func<Type, Boolean>> first, Expression<Func<Type, Boolean>> second) {
		ParameterExpression parameter = first.Parameters[default];

		SubstExpressionVisitor visitor = new() {
			Subst = { [second.Parameters[default]] = parameter }
		};

		Expression body = Expression.And(first.Body, visitor.Visit(second.Body));
		return Expression.Lambda<Func<Type, Boolean>>(body, parameter);
	}

	public static Expression<Func<Type, Boolean>> Or<Type>(this Expression<Func<Type, Boolean>> first, Expression<Func<Type, Boolean>> second) {
		ParameterExpression parameter = first.Parameters[default];

		SubstExpressionVisitor visitor = new() {
			Subst = { [second.Parameters[default]] = parameter }
		};

		Expression body = Expression.Or(first.Body, visitor.Visit(second.Body));
		return Expression.Lambda<Func<Type, Boolean>>(body, parameter);
	}

	private static Expression MakeComparison(Expression left, String comparison, String value) {
		return comparison switch {
			"==" => MakeBinary(ExpressionType.Equal, left, value),
			"!=" => MakeBinary(ExpressionType.NotEqual, left, value),
			">" => MakeBinary(ExpressionType.GreaterThan, left, value),
			">=" => MakeBinary(ExpressionType.GreaterThanOrEqual, left, value),
			"<" => MakeBinary(ExpressionType.LessThan, left, value),
			"<=" => MakeBinary(ExpressionType.LessThanOrEqual, left, value),
			"Contains"
			or "StartsWith"
			or "EndsWith"
				=> Expression.Call(
					MakeString(left),
					comparison,
					Type.EmptyTypes,
					Expression.Constant(value, typeof(String))
				),
			"In" => MakeList(left, value.Split(',')),
			_ => throw new NotSupportedException($"Invalid comparison operator '{comparison}'.")
		};
	}

	private static Expression MakeList(Expression left, IEnumerable<String> codes) {
		List<Object> objValues = codes.Cast<Object>().ToList();
		Type type = typeof(List<Object>);
		System.Reflection.MethodInfo? methodInfo = type.GetMethod("Contains", new[] { typeof(Object) });
		ConstantExpression list = Expression.Constant(objValues);
		MethodCallExpression body = Expression.Call(list, methodInfo, left);
		return body;
	}

	private static Expression MakeString(Expression source) {
		return source.Type == typeof(String) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
	}

	private static Expression MakeBinary(ExpressionType type, Expression left, String value) {
		Object? typedValue = value;
		if(left.Type != typeof(String)) {
			if(String.IsNullOrEmpty(value)) {
				typedValue = null;
				if(Nullable.GetUnderlyingType(left.Type) == null) {
					left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
				}
			} else {
				Type valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
				typedValue = valueType.IsEnum
					? Enum.Parse(valueType, value)
					: valueType == typeof(Guid)
						? Guid.Parse(value)
						: Convert.ChangeType(value, valueType);
			}
		}

		ConstantExpression right = Expression.Constant(typedValue, left.Type);
		return Expression.MakeBinary(type, left, right);
	}

	private class SubstExpressionVisitor : ExpressionVisitor {
		public readonly Dictionary<Expression, Expression> Subst = new();

		protected override Expression VisitParameter(ParameterExpression node) {
			return this.Subst.TryGetValue(node, out Expression? newValue) ? newValue : node;
		}
	}
}
