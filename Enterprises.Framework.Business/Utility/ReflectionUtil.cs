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


        /// <summary>
        /// 判断相关对象是否修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="old"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> IsUpdate<T>(T old, T current)
        {
            //Model.PerFileHistory history = new Model.PerFileHistory();
            //Model.Atrributes.ModifyFields atrr = null;
            var result = new Dictionary<string, bool>();
            Type type = typeof(T);
            PropertyInfo[] propertys = type.GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                if (property.PropertyType.IsValueType || property.PropertyType.Name == "String")
                {

                    object o1 = property.GetValue(old, null); //以前的值
                    object o2 = property.GetValue(current, null); //修改后的值
                    string str1 = o1 == null ? string.Empty : o1.ToString();
                    string str2 = o2 == null ? string.Empty : o2.ToString();
                    //判断两者是否相同，不同则插入历史表中
                    result.Add(property.Name, !str1.Equals(str2));
                }
            }

            return result;
        }
    }
}
