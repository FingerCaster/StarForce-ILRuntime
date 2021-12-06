using ILRuntime.Runtime.Enviorment;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.ValueType)]
        private static unsafe void RegisterValueTypeBinder(AppDomain appDomain)
        {
            //注册值类型绑定
            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appDomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        }
    }
}