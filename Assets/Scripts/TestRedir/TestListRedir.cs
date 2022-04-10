using System;
using System.Collections.Generic;

namespace UGFExtensions
{
    public class TestItem
    {
        public int TestInt { get; set; }
    }

    public class TestListRedir
    {
        public List<T> Test<T>(string arg)
        {
            return null;
        }
        
        public static List<object> Test(Type type,string arg)
        {
            List<object> result = new List<object>();
            for (int i = 0; i < 10; i++)
            {
                object temp = Activator.CreateInstance(type);
                var p = typeof(TestItem).GetProperty("TestInt");
                p?.SetValue(temp,i);
                result.Add(temp);
            }

            return result;
        }
    }
}