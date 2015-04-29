using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace Tests
{
    public class CloneTests
    {
        [Test]
        public void Test()
        {
            var f = new Foo
                        {
                            Name = "o",
                            Fox = new Foo { Name = "a", Fox = new Foo { Name = "b3" } },
                            Foos = new[] { new Foo { Name = "arr1" }, new Foo { Name = "arr2" }, }
                        };
            var f1 = new Foo();

            f1.InjectFrom(f);
            Assert.AreEqual(f1.Fox, f.Fox);

            f1.InjectFrom<CloneInjection>(f);

            Assert.AreNotEqual(f1.Fox, f.Fox);
            Assert.AreNotEqual(f1.Fox.Fox, f.Fox.Fox);
            Assert.AreEqual(f1.Fox.Name, f.Fox.Name);
            Assert.AreEqual(f1.Fox.Fox.Name, f.Fox.Fox.Name);

        }

        public class CloneInjection : LoopValueInjection
        {
            protected override bool AllowSetValue(object value)
            {
                return value != null;
            }

            protected override object SetValue(object sv)
            {
                if (IsGenericEnumerable(SourcePropType))
                {
                    var genArgs = SourcePropType.GetGenericArguments();
                    
                    
                    return sv;
                }
                if (SourcePropType.IsValueType || SourcePropType == typeof(string))
                    return sv;

                return Activator.CreateInstance(SourcePropType).InjectFrom<CloneInjection>(sv);
            }
        }

        static bool IsGenericEnumerable(Type t)
        {
            var genArgs = t.GetGenericArguments();
            if (genArgs.Length == 1 &&
                typeof(IEnumerable<>).MakeGenericType(genArgs).IsAssignableFrom(t)
            )
                return true;
            else
                return t.BaseType != null && IsGenericEnumerable(t.BaseType);
        }


        public class Foo
        {
            public string Name { get; set; }

            public Foo Fox { get; set; }

            public IEnumerable<Foo> Foos { get; set; }
        }

        internal static class TypeHelper
        {
            public static Type GetElementType(Type enumerableType)
            {
                return GetElementType(enumerableType, null);
            }

            public static Type GetElementType(Type enumerableType, IEnumerable enumerable)
            {
                if (enumerableType.HasElementType)
                {
                    return enumerableType.GetElementType();
                }

                if (enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                {
                    return enumerableType.GetGenericArguments()[0];
                }

                Type ienumerableType = GetIEnumerableType(enumerableType);
                if (ienumerableType != null)
                {
                    return ienumerableType.GetGenericArguments()[0];
                }

                if (typeof(IEnumerable).IsAssignableFrom(enumerableType))
                {
                    if (enumerable != null)
                    {
                        var first = enumerable.Cast<object>().FirstOrDefault();
                        if (first != null)
                            return first.GetType();
                    }
                    return typeof(object);
                }

                throw new ArgumentException(String.Format("Unable to find the element type for type '{0}'.", enumerableType), "enumerableType");
            }

            public static Type GetEnumerationType(Type enumType)
            {

                enumType = enumType.GetGenericArguments()[0];


                if (!enumType.IsEnum)
                    return null;

                return enumType;
            }

            private static Type GetIEnumerableType(Type enumerableType)
            {
                try
                {
                    return enumerableType.GetInterface("IEnumerable`1", false);
                }
                catch (System.Reflection.AmbiguousMatchException)
                {
                    if (enumerableType.BaseType != typeof(object))
                        return GetIEnumerableType(enumerableType.BaseType);

                    return null;
                }
            }
        }

    }
}