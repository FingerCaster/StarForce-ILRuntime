using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    public abstract class HotfixHelperBase : MonoBehaviour,IHotFixHelper
    {
        public abstract void Initialize();
        public abstract void Enter();
        public abstract void ShutDown();
        public abstract object CreateInstance(string typeName);
        public abstract object GetMethod(string typeName, string methodName, int paramCount);
        public abstract object InvokeMethod(object method, object instance, params object[] objects);
        public abstract T CreateHotfixMonoBehaviour<T>(GameObject go,string hotfixFullTypeName) where T : MonoBehaviour;
        public abstract Type GetHotfixType(string typeName);
        public abstract List<Type> GetAllTypes();
        public abstract object GetHotfixGameEntry { get; }
    }
}