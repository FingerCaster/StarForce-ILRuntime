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
    unsafe class UnityGameFramework_Runtime_ShowEntityFailureEventArgs_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UnityGameFramework.Runtime.ShowEntityFailureEventArgs);
            args = new Type[]{};
            method = type.GetMethod("get_ErrorMessage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ErrorMessage_0);

            field = type.GetField("EventId", flag);
            app.RegisterCLRFieldGetter(field, get_EventId_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_EventId_0, null);


        }


        static StackObject* get_ErrorMessage_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.ShowEntityFailureEventArgs instance_of_this_method = (UnityGameFramework.Runtime.ShowEntityFailureEventArgs)typeof(UnityGameFramework.Runtime.ShowEntityFailureEventArgs).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ErrorMessage;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_EventId_0(ref object o)
        {
            return UnityGameFramework.Runtime.ShowEntityFailureEventArgs.EventId;
        }

        static StackObject* CopyToStack_EventId_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UnityGameFramework.Runtime.ShowEntityFailureEventArgs.EventId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
