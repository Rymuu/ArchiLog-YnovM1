using ArchiLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

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

        public static IQueryable<TModel> Filter<TModel>(this IQueryable<TModel> query, Params p, Dictionary<string, string> arrayProperties)
        {
            //arrayProperties = arrayProperties ?? throw new ArgumentNullException(nameof(arrayProperties));
            BinaryExpression binaryExpression = null;
            var parameter = Expression.Parameter(typeof(TModel), "x");
            foreach (var item in arrayProperties)
            {
                if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
                {
                    var key = item.Key;
                    var value = item.Value;
                    var c = Expression.Constant(value);
                    var property = Expression.Property(parameter, key /*"Name"*/);
                    var o = Expression.Convert(property, typeof(object));
                    var lambda = Expression.Equal(o, c);
                    if (binaryExpression == null)
                    {
                        binaryExpression = lambda;
                    }
                    else
                        binaryExpression = Expression.And(binaryExpression, lambda);
                    // var equal = Expression.Equal(olamba,value);
                    //return await _context.Set<TModel>().Where(x => x.Name.Contains(param.Name)).ToListAsync();
                }
            }
            if (binaryExpression != null)
            {
                return query.Where(Expression.Lambda<Func<TModel, bool>>(binaryExpression, parameter));
            }
            else
                return (IQueryable<TModel>)query;
        }
       
    }
}