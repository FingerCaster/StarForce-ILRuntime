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
    unsafe class UGFExtensions_DataTableExtension_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UGFExtensions.DataTableExtension);

            field = type.GetField("DataSplitSeparators", flag);
            app.RegisterCLRFieldGetter(field, get_DataSplitSeparators_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_DataSplitSeparators_0, null);
            field = type.GetField("DataTrimSeparators", flag);
            app.RegisterCLRFieldGetter(field, get_DataTrimSeparators_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_DataTrimSeparators_1, null);


        }



        static object get_DataSplitSeparators_0(ref object o)
        {
            return UGFExtensions.DataTableExtension.DataSplitSeparators;
        }

        static StackObject* CopyToStack_DataSplitSeparators_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UGFExtensions.DataTableExtension.DataSplitSeparators;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static object get_DataTrimSeparators_1(ref object o)
        {
            return UGFExtensions.DataTableExtension.DataTrimSeparators;
        }

        static StackObject* CopyToStack_DataTrimSeparators_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = UGFExtensions.DataTableExtension.DataTrimSeparators;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
