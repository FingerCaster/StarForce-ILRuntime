using System;

namespace UGFExtensions.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CodeGeneratorAttribute : Attribute
    {
        public bool IsShow { get; }

        public CodeGeneratorAttribute(bool isShow = true)
        {
            IsShow = isShow;
        }
    }
}