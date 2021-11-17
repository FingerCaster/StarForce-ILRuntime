using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class GameFramework_Resource_LoadAssetCallbacks_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GameFramework.Resource.LoadAssetCallbacks);

            args = new Type[]{typeof(GameFramework.Resource.LoadAssetSuccessCallback), typeof(GameFramework.Resource.LoadAssetFailureCallback)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GameFramework.Resource.LoadAssetFailureCallback @loadAssetFailureCallback = (GameFramework.Resource.LoadAssetFailureCallback)typeof(GameFramework.Resource.LoadAssetFailureCallback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GameFramework.Resource.LoadAssetSuccessCallback @loadAssetSuccessCallback = (GameFramework.Resource.LoadAssetSuccessCallback)typeof(GameFramework.Resource.LoadAssetSuccessCallback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new GameFramework.Resource.LoadAssetCallbacks(@loadAssetSuccessCallback, @loadAssetFailureCallback);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
