using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Omu.ValueInjecter
{
    public class MapperInstance
    {
        /// <summary>
        /// registered custom maps
        /// </summary>
        public ConcurrentDictionary<Tuple<Type, Type>, Tuple<object, bool>> Maps = new ConcurrentDictionary<Tuple<Type, Type>, Tuple<object, bool>>();

        /// <summary>
        /// Default map used when there's no map specified for the given types
        /// </summary>
        public Func<object, Type, object, object> DefaultMap = (source, type, tag) => Activator.CreateInstance(type).InjectFrom(source);

        /// <summary>
        /// Map using default mapper ( Ignore added maps )
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="source"> source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns> mapped object</returns>
        public TResult MapDefault<TResult>(object source, object tag = null)
        {
            return (TResult)DefaultMap(source, typeof(TResult), tag);
        }

        /// <summary>
        /// Map source object to result type
        /// </summary>
        /// <typeparam name="TResult"> Result type</typeparam>
        /// <param name="source"> source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns>mapped object</returns>
        public TResult Map<TResult>(object source, object tag = null)
        {
            Tuple<object, bool> funct;
            var sourceType = source.GetType();

            Maps.TryGetValue(new Tuple<Type, Type>(sourceType, typeof(TResult)), out funct);

            if (funct != null)
            {
                var parameters = funct.Item2 ? new[] { source, tag } : new[] { source };
                return (TResult)funct.Item1.GetType().GetMethod("Invoke").Invoke(funct.Item1, parameters);
            }

            return (TResult)DefaultMap(source, typeof(TResult), tag);
        }

        /// <summary>
        /// Map source object to result type, TSource is specified not inferred, useful when source is not of TSource type like with EF proxy object
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="source">source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns>mapped object</returns>
        public TResult Map<TSource, TResult>(TSource source, object tag = null)
        {
            Tuple<object, bool> funct;
            Maps.TryGetValue(new Tuple<Type, Type>(typeof(TSource), typeof(TResult)), out funct);

            if (funct != null)
            {
                if (funct.Item2)
                    return ((Func<TSource, object, TResult>)funct.Item1)(source, tag);
                return ((Func<TSource, TResult>)funct.Item1)(source);
            }

            return (TResult)DefaultMap(source, typeof(TResult), tag);
        }

        /// <summary>
        /// register a function for mapping from source to result
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="func">function receives source and returns result object</param>
        public void AddMap<TSource, TResult>(Func<TSource, TResult> func)
        {
            Maps.AddOrUpdate(new Tuple<Type, Type>(typeof(TSource), typeof(TResult)), new Tuple<object, bool>(func, false), (key, oldValue) => new Tuple<object, bool>(func, false));
        }

        /// <summary>
        /// register a function for mapping from source to result
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="func">function receives source, tag object for additional parameters, and returns result object</param>
        public void AddMap<TSource, TResult>(Func<TSource, object, TResult> func)
        {
            Maps.AddOrUpdate(new Tuple<Type, Type>(typeof(TSource), typeof(TResult)), new Tuple<object, bool>(func, true), (key, oldValue) => new Tuple<object, bool>(func, true));
        }

        /// <summary>
        /// clear added maps and reset default map to initial value
        /// </summary>
        public void Reset()
        {
            Maps.Clear();
            DefaultMap = (source, type, tag) => Activator.CreateInstance(type).InjectFrom(source);
        }
    }
}