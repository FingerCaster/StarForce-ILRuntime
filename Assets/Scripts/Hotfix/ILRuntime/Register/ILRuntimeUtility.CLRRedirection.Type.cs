using System;
using System.Collections.Generic;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static unsafe void RegisterTypeCLRRedirection(AppDomain appDomain)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                                BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Type);
            args = new Type[] { };
            method = type.GetMethod("get_IsInterface", flag, null, args, null);
            appDomain.RegisterCLRMethodRedirection(method, get_IsInterface_5);
        }

        private static unsafe StackObject* get_IsInterface_5(ILIntepreter __intp, StackObject* __esp,
            IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Type instance_of_this_method = (System.Type)typeof(System.Type).CheckCLRTypes(
                StackObject.ToObject(ptr_of_this_method, __domain, __mStack),
                (ILRuntime.CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);
            bool result_of_this_method;

            if (instance_of_this_method is ILRuntimeType ilRuntimeType)
            {
                result_of_this_method = ilRuntimeType.ILType.IsInterface;
            }
            else
            {
                result_of_this_method = instance_of_this_method.IsInterface;
            }

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }
    }
}