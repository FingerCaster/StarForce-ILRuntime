using System;

namespace UGFExtensions.Hotfix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ILRunTimeRegisterAttribute : Attribute
    {
        public ILRegister ILRegister { get;}

        public ILRunTimeRegisterAttribute(ILRegister ilRegister)
        {
            ILRegister = ilRegister;
        }
    }

    /// <summary>
    /// ILRuntime 需要注册的类型
    /// </summary>
    public enum ILRegister
    {
        /// <summary>
        /// CLR 重定向
        /// </summary>
        Redirection,
        /// <summary>
        /// 委托
        /// </summary>
        Delegate,
        /// <summary>
        /// 值类型
        /// </summary>
        ValueType,
        /// <summary>
        /// 适配器
        /// </summary>
        Adaptor,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
    }
}