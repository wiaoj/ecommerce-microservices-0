using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Core.Persistence.EfCore;

// https://github.com/nreco/lambdaparser
// https://github.com/dynamicexpresso/DynamicExpresso
public static class EfCoreQueryableExtensions {
	public static Task<ListResultModel<Type>> ApplyPagingAsync<Type>(this IQueryable<Type> collection, CancellationToken cancellationToken) {
		return ApplyPagingAsync(collection, 1, 10, cancellationToken);
	}
	public static async Task<ListResultModel<Type>> ApplyPagingAsync<Type>(
		this IQueryable<Type> collection,
		Int32 page,
		Int32 pageSize,
		CancellationToken cancellationToken) where Type : notnull {
		if(page <= default(Int32)) {
			page = 1;
		}

		if(pageSize <= default(Int32)) {
			pageSize = 10;
		}

		Boolean isEmpty = await collection.AnyAsync(cancellationToken: cancellationToken) == false;
		if(isEmpty) {
			return ListResultModel<Type>.Empty;
		}

		Int32 totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
		Int32 totalPages = (Int32)Math.Ceiling((Decimal)totalItems / pageSize);
		List<Type> data = await collection.Limit(page, pageSize).ToListAsync(cancellationToken: cancellationToken);

		return ListResultModel<Type>.Create(data, totalItems, page, pageSize);
	}

	public static Task<ListResultModel<TypeResult>> ApplyPagingAsync<Type, TypeResult>(
		this IQueryable<Type> collection,
		IConfigurationProvider configuration,
		CancellationToken cancellationToken) where TypeResult : notnull {
		return ApplyPagingAsync<Type,TypeResult>(collection, configuration, 1, 10, cancellationToken);
	}

	public static async Task<ListResultModel<TypeResult>> ApplyPagingAsync<Type, TypeResult>(
		this IQueryable<Type> collection,
		IConfigurationProvider configuration,
		Int32 page,
		Int32 pageSize,
		CancellationToken cancellationToken) where TypeResult : notnull {
		if(page <= default(Int32)) {
			page = 1;
		}

		if(pageSize <= default(Int32)) {
			pageSize = 10;
		}

		Boolean isEmpty = await collection.AnyAsync(cancellationToken: cancellationToken) == false;
		if(isEmpty) {
			return ListResultModel<TypeResult>.Empty;
		}

		Int32 totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
		Int32 totalPages = (Int32)Math.Ceiling((Decimal)totalItems / pageSize);
		List<TypeResult> data = await collection.Limit(page, pageSize).ProjectTo<TypeResult>(configuration)
			.ToListAsync(cancellationToken: cancellationToken);

		return ListResultModel<TypeResult>.Create(data, totalItems, page, pageSize);
	}

	public static IQueryable<TypeEntity> ApplyPaging<TypeEntity>(this IQueryable<TypeEntity> source, Int32 page, Int32 size) where TypeEntity : class {
		return source.Skip(page * size).Take(size);
	}

	public static IQueryable<Type> Limit<Type>(this IQueryable<Type> collection) {
		return Limit(collection, 1, 10);
	}

	public static IQueryable<Type> Limit<Type>(this IQueryable<Type> collection, Int32 page, Int32 resultsPerPage) {
		if(page <= default(Int32)) {
			page = 1;
		}

		if(resultsPerPage <= default(Int32)) {
			resultsPerPage = 10;
		}

		Int32 skip = (page - 1) * resultsPerPage;
		IQueryable<Type> data = collection.Skip(skip).Take(resultsPerPage);

		return data;
	}

	public static IQueryable<TEntity> ApplyFilter<TEntity>(
		this IQueryable<TEntity> source,
		IEnumerable<FilterModel>? filters) where TEntity : class {
		if(filters is null) {
			return source;
		}

		List<Expression<Func<TEntity, Boolean>>> filterExpressions = new();

		foreach((String fieldName, String comparision, String fieldValue) in filters) {
			Expression<Func<TEntity, Boolean>> expr = PredicateBuilder.Build<TEntity>(fieldName, comparision, fieldValue);
			filterExpressions.Add(expr);
		}

		return source.Where(filterExpressions.Aggregate((expr1, expr2) => expr1.And(expr2)));
	}

	public static IQueryable<TEntity> ApplyIncludeList<TEntity>(
		this IQueryable<TEntity> source,
		IEnumerable<String>? navigationPropertiesPath) where TEntity : class {
		if(navigationPropertiesPath is null) {
			return source;
		}

		foreach(String navigationPropertyPath in navigationPropertiesPath) {
			source = source.Include(navigationPropertyPath);
		}

		return source;
	}
}