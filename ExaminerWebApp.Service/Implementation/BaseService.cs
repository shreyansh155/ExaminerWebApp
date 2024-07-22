using ExaminerWebApp.Composition.Helpers;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;


namespace ExaminerWebApp.Service.Implementation
{
    public class BaseService<T>
    {
        protected static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, GridFilter filter)
        {
            if (filter.Operator == "eq")
            {
                query = query.Where($"{filter.Field}.ToLower() == @0", filter.Value.ToString().ToLower());
            }
            else if (filter.Operator == "neq")
            {
                query = query.Where($"{filter.Field}.ToLower() != @0", filter.Value.ToString().ToLower());
            }
            else if (filter.Operator == "contains")
            {
                query = query.Where($"{filter.Field}.ToLower().Contains(@0)", filter.Value.ToString().ToLower());
            }

            return query;
        }

        protected static IQueryable<T> ApplySorting(IQueryable<T> query, GridSort sort)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sort.Field);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = sort.Dir == "asc" ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { query.ElementType, property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
