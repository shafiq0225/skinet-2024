using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == "React"
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // x => x.Price
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // x => x.Name
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct(); // Apply distinct if specified
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take); // Apply paging if specified
        }

        
        query = spec.Includes.Aggregate(query, (current, include) =>
            current.Include(include));

        query = spec.IncludeStrings.Aggregate(query, (current, include) =>
            current.Include(include));

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == "React"
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // x => x.Price
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // x => x.Name
        }

        var selectQuery = query as IQueryable<TResult>;

        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select); // x => new { x.Name, x.Price }
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct(); // Apply distinct if specified
        }

        if (spec.IsPagingEnabled)
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take); // Apply paging if specified
        }
        return selectQuery ?? query.Cast<TResult>();
    }
}