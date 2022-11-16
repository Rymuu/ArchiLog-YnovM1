using ArchiLibrary.Models;
using System.Linq.Expressions;


namespace ArchiLibrary.Extensions
{
    public static class QueryExtensions
    {

        public static IOrderedQueryable<TModel> Sort<TModel>(this IQueryable<TModel> query, Params p)
        {
            if (!string.IsNullOrWhiteSpace(p.Asc))
            {
                string champ = p.Asc;

                //créer lambda
                var parameter = Expression.Parameter(typeof(TModel), "x");
                var property = Expression.Property(parameter, champ/*"Name"*/);

                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilisation lambda
                return query.OrderBy(lambda);
                //return query.OrderBy(x => x.Name);
            }
            else if (!string.IsNullOrWhiteSpace(p.Desc))
            {
                string champ = p.Desc;

                //créer lambda
                var parameter = Expression.Parameter(typeof(TModel), "x");
                var property = Expression.Property(parameter, champ/*"Name"*/);

                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilisation lambda
                return query.OrderByDescending(lambda);
                //return query.OrderBy(x => x.Name);
            }
            
            else
                return (IOrderedQueryable<TModel>)query;

        }

        public static IOrderedQueryable<TModel> Pagination<TModel>(this IQueryable<TModel> query, int start, int end)
        { 
                return (IOrderedQueryable<TModel>)query.Skip(start).Take((end-start) + 1);
                //return query.OrderBy(x => x.Name);
        }
    }
}