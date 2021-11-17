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
    unsafe class GameFramework_ObjectPool_IObjectPool_1_UGFExtensions_ObjectBaseAdapter_Binding_Adapter_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>);
            args = new Type[]{typeof(System.Object)};
            method = type.GetMethod("Unspawn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Unspawn_0);
            args = new Type[]{};
            method = type.GetMethod("Spawn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Spawn_1);
            args = new Type[]{typeof(UGFExtensions.ObjectBaseAdapter.Adapter), typeof(System.Boolean)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_2);


        }


        static StackObject* Unspawn_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @target = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter> instance_of_this_method = (GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>)typeof(GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Unspawn(@target);

            return __ret;
        }

        static StackObject* Spawn_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter> instance_of_this_method = (GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>)typeof(GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Spawn();

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Register_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @spawned = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UGFExtensions.ObjectBaseAdapter.Adapter @obj = (UGFExtensions.ObjectBaseAdapter.Adapter)typeof(UGFExtensions.ObjectBaseAdapter.Adapter).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter> instance_of_this_method = (GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>)typeof(GameFramework.ObjectPool.IObjectPool<UGFExtensions.ObjectBaseAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Register(@obj, @spawned);

            return __ret;
        }



    }
}
