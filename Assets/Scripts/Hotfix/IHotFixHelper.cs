using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    public interface IHotFixHelper
    {
        void Initialize();
        void Enter();
        void ShutDown();
        object CreateInstance(string typeName);
        object GetMethod(string typeName,string methodName,int paramCount);
        object InvokeMethod(object method, object instance, params object[] objects);
        T CreateHotfixMonoBehaviour<T>(GameObject go,string hotfixFullTypeName)  where T : MonoBehaviour;
        Type GetHotfixType(string typeName);
        List<Type> GetAllTypes();

        object GetHotfixGameEntry { get; }
    }
}