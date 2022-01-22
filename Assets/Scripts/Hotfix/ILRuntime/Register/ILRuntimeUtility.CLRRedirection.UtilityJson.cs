using System;
using System.Reflection;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static unsafe void RegisterUtilityJsonCLRRedirection(AppDomain appDomain)
        {
            if (!Application.isPlaying) return;
            var fieldInfo = typeof(BaseComponent).GetField("m_JsonHelperTypeName",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                return;
            }

            BaseComponent baseComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            string jsonHelperTypeName = (string)fieldInfo.GetValue(baseComponent);
            if (jsonHelperTypeName == "UGFExtensions.Helper.CatJsonHelper")
            {
                foreach (var i in typeof(Utility.Json).GetMethods())
                {
                    if (i.Name == "ToObject" && i.IsGenericMethodDefinition)
                    {
                        appDomain.RegisterCLRMethodRedirection(i, CatJson.JsonParser.RedirectionParseJson);
                    }

                    if (i.Name == "ToJson" && i.IsGenericMethodDefinition)
                    {
                        appDomain.RegisterCLRMethodRedirection(i, CatJson.JsonParser.RedirectionToJson);
                    }
                }
            }
            else
            {
                throw new Exception("On ILRuntime Use Utility.Json Please Register ILRuntime CLRRedirection!");
            }
        }
    }
}