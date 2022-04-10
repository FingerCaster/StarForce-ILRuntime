using System;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static unsafe void RegisterTestListRedirCLRRedirection(AppDomain appDomain)
        {
            var arr = typeof(TestListRedir).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "Test" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, TestListRedirTest);
                }
            }
        }

        // ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
        // StackObject* ptr_of_this_method;
        // StackObject* __ret = ILIntepreter.Minus(esp, 1);
        // ptr_of_this_method = ILIntepreter.Minus(esp, 1);
        // //
        // System.String selection = (System.String) typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, mStack));
        // intp.Free(ptr_of_this_method);
        // var type = method.GenericArguments[0].ReflectionType;
        // //调用
        // var result_of_this_method = DB.GetTableRuntime().FormAll(type, selection);
        //     if (type == typeof(ILTypeInstance))
        // {
        //     //转成ilrTypeInstance
        //     var retList = new List<ILTypeInstance>(result_of_this_method.Count);
        //     for (int i = 0; i < result_of_this_method.Count; i++)
        //     {
        //         var hotfixObj = result_of_this_method[i] as ILTypeInstance;
        //         retList.Add(hotfixObj);
        //     }
        //
        //     return ILIntepreter.PushObject(__ret, mStack, retList);
        private static unsafe StackObject* TestListRedirTest(ILIntepreter __intp, StackObject* __esp,
            IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            //
            System.String selection = (System.String) typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);
            var type = __method.GenericArguments[0];
            Log.Warning(type);
            Log.Warning(selection);

            var clrType = type.TypeForCLR;
            var result = TestListRedir.Test(clrType, selection);
            var listType = typeof(List<>);
            var genericType = listType.MakeGenericType(clrType);
    
            object result1 = Activator.CreateInstance(genericType);
            var addMethod = genericType.GetMethod("Add");
            object[] objs = new object[1]; 
            foreach (var item in result)
            {
                objs[0] = item;
                addMethod?.Invoke(result1,objs);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result1);
 
        }
    }
}