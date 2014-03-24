using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Enterprises.Framework.Utility
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectionUtil
    {
        /// <summary>
        ///  获取枚举类型的描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(Type type, int value)
        {
            try
            {
                FieldInfo field = type.GetField(Enum.GetName(type, value));
                if (field == null)
                {
                    return "无";
                }

                var desc = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (desc != null) return desc.Description;
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取枚举值和描述集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SortedList<int, string> GetEnumerators(Type type)
        {
            var list = new SortedList<int, string>();

            foreach (int value in Enum.GetValues(type))
            {
                list.Add(value, GetDescription(type, value));
            }

            return list;
        }


        /// <summary>
        /// 判断目标泛型类型是否实现了某个接口，如果实现了该接口则提取泛型元素类型
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
        {
            Func<Type, bool> predicate = t => t.IsGenericType && (t.GetGenericTypeDefinition() == interfaceType);
            if (!predicate(queryType))
            {
                return queryType.GetInterfaces().FirstOrDefault<Type>(predicate);
            }

            return queryType;
        }

        /// <summary>
        /// 获取某个类的某个属性（Property）的Attribute名称
        /// </summary>
        /// <param name="modelType">类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static T GetAttribute<T>(Type modelType, string propertyName) where T : class
        {
            var requiredAttribute = TypeDescriptor.GetProperties(modelType)[propertyName].Attributes[typeof(T)] as T;
            if (requiredAttribute != null)
            {
                return requiredAttribute;
            }

            return null;
        }

        /// <summary>
        /// 判断某个类型的某个属性（Property）是否有Attribute属性
        /// </summary>
        /// <typeparam name="T">属性类</typeparam>
        /// <param name="modelType">类型</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static bool HasAttribute<T>(Type modelType, string propertyName) where T : class
        {
            var displayNameAttribute = TypeDescriptor.GetProperties(modelType)[propertyName].Attributes[typeof(T)] as T;
            return displayNameAttribute != null;
        }
    }
}
