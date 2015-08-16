using System;

namespace Omu.ValueInjecter
{
    public class Mapper
    {
        public static MapperInstance Instance = new MapperInstance();
      
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

        public static TResult MapDefault<TResult>(object source, object tag = null)
        {
            return Instance.MapDefault<TResult>(source, tag);
        }

        public static TResult Map<TResult>(object source, object tag = null)
        {
            return Instance.Map<TResult>(source, tag);
        }

        public static TResult Map<TSource, TResult>(TSource source, object tag = null)
        {
            return Instance.Map<TSource, TResult>(source, tag);
        }

        public static void AddMap<TSource, TResult>(Func<TSource, TResult> func)
        {
            Instance.AddMap<TSource, TResult>(func);
        }

        public static void AddMap<TSource, TResult>(Func<TSource, object, TResult> func)
        {
            Instance.AddMap<TSource, TResult>(func);
        }

        public static void Reset()
        {
            Instance.Reset();
        }
    }
}