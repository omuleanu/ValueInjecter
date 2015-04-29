using System;

namespace DALSample
{
    public static class TableConvention
    {
        public static string Resolve(Type t)
        {
            var name = t.Name;
            if (name.EndsWith("s")) return t.Name + "es";
            return t.Name + "s";
        }

        public static string Resolve(object o)
        {
            return Resolve(o.GetType());
        }
    }
}