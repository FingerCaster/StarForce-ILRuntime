using System;
using System.Collections.Generic;
using System.IO;
using UGFExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    [CodeGenerator]
    public class EventArgsCodeGenerator : CodeGeneratorBase
    {
        private enum EventArgType
        {
            Object,
            Int,
            Float,
            Bool,
            Char,
            String,

            UnityObject,
            GameObject,
            Transform,
            Vector2,
            Vector3,
            Quaternion,

            Other,
        }

        /// <summary>
        /// 事件参数数据
        /// </summary>
        [Serializable]
        private class EventArgsData
        {
            public string Type;
            public string Name;
            public EventArgType TypeEnum;

            public EventArgsData()
            {
            }

            public EventArgsData(string type, string name)
            {
                Type = type;
                Name = name;
            }
        }

        /// <summary>
        /// 事件参数数据列表
        /// </summary>
        private List<EventArgsData> m_EventArgsDatas = new List<EventArgsData>();

        public EventArgsCodeGenerator() : base("Assets/Scripts/HotfixExtenison/GameMain/EventArgs", "EventArgs")
        {
            m_EventArgsDatas.Clear();
        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("事件参数类名：", GUILayout.Width(140f));
            CodeName = EditorGUILayout.TextField(CodeName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("热更新层事件：", GUILayout.Width(140f));
            IsHotfix = EditorGUILayout.Toggle(IsHotfix);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码路径：", GUILayout.Width(140f));
            EditorGUILayout.LabelField(GetCodeFolder());
            EditorGUILayout.EndHorizontal();

            //绘制事件参数相关按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Add(new EventArgsData(null, null));
            }

            if (GUILayout.Button("删除所有事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Clear();
            }

            if (GUILayout.Button("删除空事件参数", GUILayout.Width(140f)))
            {
                for (int i = m_EventArgsDatas.Count - 1; i >= 0; i--)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    if (string.IsNullOrWhiteSpace(data.Name))
                    {
                        m_EventArgsDatas.RemoveAt(i);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            //绘制事件参数数据
            for (int i = 0; i < m_EventArgsDatas.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EventArgsData data = m_EventArgsDatas[i];
                EditorGUILayout.LabelField("参数类型：", GUILayout.Width(70f));
                data.TypeEnum = (EventArgType)EditorGUILayout.EnumPopup(data.TypeEnum, GUILayout.Width(100f));
                switch (data.TypeEnum)
                {
                    case EventArgType.Object:
                    case EventArgType.Int:
                    case EventArgType.Float:
                    case EventArgType.Bool:
                    case EventArgType.Char:
                    case EventArgType.String:
                        data.Type = data.TypeEnum.ToString().ToLower();
                        break;

                    case EventArgType.UnityObject:
                        data.Type = "UnityEngine.Object";
                        break;

                    case EventArgType.Other:
                        data.Type = EditorGUILayout.TextField(data.Type, GUILayout.Width(140f));
                        break;

                    default:
                        data.Type = data.TypeEnum.ToString();
                        break;
                }

                EditorGUILayout.LabelField("参数字段名：", GUILayout.Width(70f));
                data.Name = EditorGUILayout.TextField(data.Name, GUILayout.Width(140f));
                EditorGUILayout.EndHorizontal();
            }
        }

        public override bool GenCode()
        {
            //根据是否为热更新层事件来决定一些参数
            string codePath = GetCodePath();
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            string baseClass = IsHotfix ? "HotfixGameEventArgs" : "GameEventArgs";

            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }

            using (StreamWriter sw = new StreamWriter(codePath))
            {
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("using GameFramework.Event;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic class {CodeName} : {baseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //事件编号
                sw.WriteLine($"\t\tpublic static readonly int EventId = typeof({CodeName}).GetHashCode();");
                sw.WriteLine("");
                sw.WriteLine("\t\tpublic override int Id");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tget");
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn EventId;");
                sw.WriteLine("\t\t\t}");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //事件参数
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    data.Name = data.Name[0].ToString().ToUpper() + data.Name.Substring(1);
                    sw.WriteLine($"\t\tpublic {data.Type} {data.Name}");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tget;");
                    sw.WriteLine("\t\t\tprivate set;");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("");
                }

                //清空参数数据方法
                sw.WriteLine($"\t\tpublic override void Clear()");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = default({data.Type});");
                }

                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //填充参数数据方法
                sw.Write($"\t\tpublic {CodeName} Fill(");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.Write($"{data.Type} {data.Name.ToLower()}");
                    if (i != m_EventArgsDatas.Count - 1)
                    {
                        sw.Write(",");
                    }
                }

                sw.WriteLine(")");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = {data.Name.ToLower()};");
                }

                sw.WriteLine("\t\t\treturn this;");
                sw.WriteLine("\t\t}");


                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
            
            return true;
        }
    }
}