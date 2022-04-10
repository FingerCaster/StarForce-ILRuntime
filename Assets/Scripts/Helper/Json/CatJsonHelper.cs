using System;
using GameFramework;
using CatJson;

namespace UGFExtensions.Helper
{
    public class CatJsonHelper : Utility.Json.IJsonHelper
    {
        public string ToJson(object obj)
        {
           return JsonParser.ToJson(obj);
        }

        public T ToObject<T>(string json)
        {
            return JsonParser.ParseJson<T>(json);
        }

        public object ToObject(Type objectType, string json)
        {
            return JsonParser.ParseJson(json,objectType);
        }
    }
}