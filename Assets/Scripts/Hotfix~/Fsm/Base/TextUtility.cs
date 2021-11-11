using System;
using GameFramework;

namespace UGFExtensions.Hotfix
{
    public static class TextUtility
    {
        /// <summary>根据类型和名称获取完整名称。</summary>
        /// <typeparam name="T">类型。</typeparam>
        /// <param name="name">名称。</param>
        /// <returns>完整名称。</returns>
        public static string GetFullName<T>(string name)
        {
            return GetFullName(typeof(T), name);
        }

        /// <summary>根据类型和名称获取完整名称。</summary>
        /// <param name="type">类型。</param>
        /// <param name="name">名称。</param>
        /// <returns>完整名称。</returns>
        public static string GetFullName(Type type, string name)
        {
            if (type == null)
                throw new GameFrameworkException("Type is invalid.");
            var fullName = type.FullName;
            if (!string.IsNullOrEmpty(name))
                return Utility.Text.Format("{0}.{1}", fullName, name);
            return fullName;
        }
    }
}