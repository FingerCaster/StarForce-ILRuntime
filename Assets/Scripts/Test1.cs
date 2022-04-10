using System;
using System.IO;
using System.Reflection;
using UGFExtensions.Hotfix;
using UnityEngine;

namespace UGFExtensions
{
    public class Test1 : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log(123123);
            Assembly assembly = Assembly.Load(File.ReadAllBytes("Assets/Res/HotfixDll/Hotfix.dll.bytes") );
            var type = assembly.GetType("UGFExtensions.Hotfix.TestList");
            object obj = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");
            method.Invoke(obj,null);
        }
    }
}