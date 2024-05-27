using System;

namespace Omu.ValueInjecter
{
    /// <summary>
    /// mapper instance class
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// static mapper instance
        /// </summary>
        public static MapperInstance Instance = new MapperInstance();

        /// <summary>
        /// Default map used when there's no map specified for the given types
        /// </summary>
        public static Func<object, Type, object, object> DefaultMap
        {
            get
            {
                return Instance.DefaultMap;
            }
            set
            {
                Instance.DefaultMap = value;
            }
        }

        /// <summary>
        /// Map using default mapper ( Ignore added maps )
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="source"> source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns> mapped object</returns>
        public static TResult MapDefault<TResult>(object source, object tag = null)
        {
            return Instance.MapDefault<TResult>(source, tag);
        }

        /// <summary>
        /// Map source object to result type
        /// </summary>
        /// <typeparam name="TResult"> Result type</typeparam>
        /// <param name="source"> source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns>mapped object</returns>
        public static TResult Map<TResult>(object source, object tag = null)
        {
            return Instance.Map<TResult>(source, tag);
        }

        /// <summary>
        /// Map source object to result type, TSource is specified not inferred, useful when source is not of TSource type like with EF proxy object
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="source">source object</param>
        /// <param name="tag">object used to send additional paramaters for the mapping code</param>
        /// <returns>mapped object</returns>
        public static TResult Map<TSource, TResult>(TSource source, object tag = null)
        {
            return Instance.Map<TSource, TResult>(source, tag);
        }

        /// <summary>
        /// register a function for mapping from source to result
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="func">function receives source and returns result object</param>
        public static void AddMap<TSource, TResult>(Func<TSource, TResult> func)
        {
            Instance.AddMap<TSource, TResult>(func);
        }

        /// <summary>
        /// register a function for mapping from source to result
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="func">function receives source, tag object for additional parameters, and returns result object</param>
        public static void AddMap<TSource, TResult>(Func<TSource, object, TResult> func)
        {
            Instance.AddMap<TSource, TResult>(func);
        }

        /// <summary>
        /// clear added maps and default map
        /// </summary>
        public static void Reset()
        {
            Instance.Reset();
        }
    }
}