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
            ConstantExpression c = null;

            var parameter = Expression.Parameter(typeof(TModel), "x");
            UnaryExpression o = null;
            foreach (var item in arrayProperties)
            {
                if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
                {
                    var key = item.Key;
                    var value = item.Value;
                    c = Expression.Constant(value);
                    var property = Expression.Property(parameter, key /*"Name"*/);

                    //choix du type de la valeur
                    if (property.Type == typeof(string))
                    {
                        o = Expression.Convert(property, typeof(string));
                    }
                    else if (property.Type == typeof(int))
                    {
                        o = Expression.Convert(property, typeof(int));
                    }
                    else if (property.Type == typeof(DateTime))
                    {
                        if (item.Value.Contains(",") == false)
                        {
                            c = Expression.Constant(DateTime.Parse(value));
                        }
                        o = Expression.Convert(property, typeof(DateTime));
                    }
                    BinaryExpression? lambda = null;
                    //savoir si il y'a la value possède 2 element
                    if (item.Value.Contains(","))
                    {
                        var index = value.IndexOf(",");
                        string[] values = item.Value.Split(",");
                        string? before = null;
                        string? after = null;
                        var type = property.GetType();
                        if (index != -1 && property.Type == typeof(int))
                        {
                            before = value.Substring(0, index);
                            after = value.Substring(index, value.Length - 1);
                        }
                        //valeur inferieur ou égal
                        if (before != null && before == "")
                        {
                            if (o.Type == typeof(int))
                            {
                                lambda = Expression.LessThanOrEqual(property, Expression.Constant(int.Parse(values[1])));
                            }
                            if (o.Type == typeof(string))
                            {
                                lambda = Expression.Equal(property, Expression.Constant(values[1].ToString()));
                            }
                            if (o.Type == typeof(DateTime))
                            {
                                lambda = Expression.LessThanOrEqual(property, Expression.Constant(DateTime.Parse(values[1])));
                            }

                        }
                        else if (after != null && after == "," && o.Type != typeof(string))
                        {
                            if (o.Type == typeof(int))
                            {
                                lambda = Expression.GreaterThanOrEqual(property, Expression.Constant(int.Parse(values[0])));
                            }
                            if (o.Type == typeof(string))
                            {
                                lambda = Expression.Equal(property, Expression.Constant(values[0].ToString()));
                            }
                            if (o.Type == typeof(DateTime))
                            {
                                lambda = Expression.GreaterThanOrEqual(property, Expression.Constant(DateTime.Parse(values[0])));
                            }
                        }
                        else
                        {
                            if (o.Type == typeof(int))
                            {
                                lambda = Expression.And(Expression.GreaterThanOrEqual(o, Expression.Constant(int.Parse(values[0]))), Expression.LessThanOrEqual(o, Expression.Constant(int.Parse(values[1]))));
                            }
                            if (o.Type == typeof(string))
                            {
                                lambda = Expression.Or(Expression.Equal(o, Expression.Constant(values[0].ToString())), Expression.Equal(o, Expression.Constant(values[1].ToString())));
                            }
                            if (o.Type == typeof(DateTime))
                            {
                                Console.WriteLine(DateTime.Parse(values[0]).GetType());
                                lambda = Expression.Or(Expression.Equal(o, Expression.Constant(DateTime.Parse(values[0]))), Expression.Equal(o, Expression.Constant(DateTime.Parse(values[1]))));
                            }
                            //lambda = Expression.And(Expression.GreaterThan(o, Expression.Constant(values[0])), Expression.LessThan(o, Expression.Constant(values[1])));

                        }
                        //Expression.LessThan(o, Expression.Constant(values[0])), Expression.Equal(o, Expression.Constant(values[1]));
                    }

                    else
                    {

                        if (o.Type == typeof(DateTime))
                        {
                            lambda = Expression.Equal(o, c);
                        }
                        else
                        {
                            lambda = Expression.Equal(o, c);
                        }

                    }

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