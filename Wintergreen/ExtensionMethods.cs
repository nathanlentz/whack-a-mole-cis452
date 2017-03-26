using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintergreen.ExtensionMethods
{
    public static class ExtensionMethods
    {
        #region string

        /// <summary>
        /// Indicates whether the <see cref="string"/> is null or a string.empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Indicates whether the <see cref="string"/> is null, empty, or consists only of white-space characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        #endregion string

        #region List

        /// <summary>
        /// Adds an object to a list and returns the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> AddAndReturn<T>(this List<T> list, T data)
        {
            list.Add(data);
            return list;
        }

        #endregion List

        #region IComparable

        /// <summary>
        /// Clamps an <see cref="IComparable"/> value into the specified range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="min">A minimum value of type <see cref="T"/>.</param>
        /// <param name="max">A maximum value of type <see cref="T"/>.</param>
        /// <returns></returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) <= 0)
                return min;
            else if (val.CompareTo(max) >= 0)
                return max;
            else
                return val;
        }

        #endregion

        #region Type

        /// <summary>
        /// The <see cref="Type"/> directly inherited from <see cref="Component"/> in the class's <see cref="Type"/> hierarchy.
        /// Throws an exception if <see cref="Type"/> does not inherit from <see cref="Component"/> at all.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="Component"/> to check.</param>
        /// <returns></returns>
        internal static Type BaseComponentType(this Type type)
        {
            Type baseType = type.BaseType;
            if (baseType == null || baseType == typeof(object))
                throw new Exception("Type must derive from Component to have a BaseComponentType");
            else if (baseType == typeof(Component))
                return type;

            while (baseType != null)
            {
                if (baseType.BaseType == typeof(Component))
                    return baseType;

                baseType = baseType.BaseType;
            }
            throw new Exception("Type must derive from Component to have a BaseComponentType");
        }

        #endregion
    }
}
