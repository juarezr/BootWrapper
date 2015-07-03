using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BootWrapper.Mvc.Core
{
    public static class MethodsHelpers
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || String.IsNullOrWhiteSpace(obj.ToString());
        }

        public static bool HasAllEmptyProperties(this object obj)
        {
            var type = obj.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var hasProperty = properties.Select(x => x.GetValue(obj, null))
                                        .Any(x => !x.IsNullOrEmpty());
            return !hasProperty;
        }
    }
}
