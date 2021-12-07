#if ILRuntime
using System;
using System.Collections.Generic;
using CatJson;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;

namespace UGFExtensions
{
    public static class CatJsonExtensions
    {
        public unsafe static StackObject* RedirectionParseJson(ILIntepreter intp, StackObject* esp,
            IList<object> mStack, CLRMethod method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(esp, 1);
            bool reflection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(esp, 2);
            string json = (string)typeof(string).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, mStack));

            intp.Free(ptr_of_this_method);

            Type type = method.GenericArguments[0].ReflectionType;

            object result_of_this_method = JsonParser.ParseJson(json, type,reflection);

            return ILIntepreter.PushObject(__ret, mStack, result_of_this_method);
        }
        public unsafe static StackObject* RedirectionToJson(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(esp, 1);
            bool reflection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(esp, 2);
            ILTypeInstance obj =
                (ILTypeInstance)typeof(ILTypeInstance).CheckCLRTypes(
                    StackObject.ToObject(ptr_of_this_method, __domain, mStack), 0);

            intp.Free(ptr_of_this_method);

            Type type = method.GenericArguments[0].ReflectionType;

            string result_of_this_method = JsonParser.ToJson(obj, type, reflection);

            return ILIntepreter.PushObject(__ret, mStack, result_of_this_method);
        }
    }
}
#endif
