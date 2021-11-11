using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UGFExtensions.Editor
{
    [CodeGenerator]
    public class BindComponentCodeGenerator : CodeGeneratorBase
    {
        private readonly GameObjectRecordableList m_GameObjectRecordableList;
        private bool m_IsPartial = false;

        public bool IsPartial
        {
            get => m_IsPartial;
            set => m_IsPartial = value;
        }

        /// <summary>
        /// 是否生成自动绑定组件属性代码
        /// </summary>
        private bool m_IsGenAutoBindCodeProperty = false;

        public bool IsGenAutoBindCodeProperty
        {
            get => m_IsGenAutoBindCodeProperty;
            set => m_IsGenAutoBindCodeProperty = value;
        }

        public BindComponentCodeGenerator(string mainCodeFolder, string hotfixCodeFolder,bool isHotfix) : base(mainCodeFolder,
            hotfixCodeFolder)
        {
            IsHotfix = isHotfix;
            Suffix = ".BindComponents.cs";
            m_GameObjectRecordableList = new GameObjectRecordableList();
        }

        public BindComponentCodeGenerator() : base("Assets/Scripts/BindComponents", "BindComponents")
        {
            Suffix = ".BindComponents.cs";
            m_GameObjectRecordableList = new GameObjectRecordableList();
        }

        public override void Draw()
        {
            m_GameObjectRecordableList.HandleGameObjectsListUI();
            IsHotfix = EditorGUILayout.ToggleLeft("热更层代码", IsHotfix);
            m_IsGenAutoBindCodeProperty = EditorGUILayout.ToggleLeft("生成属性代码", m_IsGenAutoBindCodeProperty);
            EditorGUILayout.LabelField("自动生成代码路径", GetCodeFolder());
        }
        private static void AddUsingNameSpace(StreamWriter sw, ComponentAutoBindTool bindTool)
        {
            List<string> nameSpaces = new List<string>();
            foreach (ComponentAutoBindTool.BindData data in bindTool.BindDatas)
            {
                System.Type type = data.BindCom.GetType();
                nameSpaces.Add(type.Namespace);
            }
            nameSpaces = nameSpaces.Distinct().ToList();
            foreach (string nameSpace in nameSpaces)
            {
                sw.WriteLine($"using {nameSpace};");
            }
        }


        public void GenCode(ComponentAutoBindTool bindTool)
        {
            if (bindTool == null)
            {
                return;
            }
            string codePath = GetCodePath();
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }

            using (StreamWriter sw = new StreamWriter(codePath))
            {
                AddUsingNameSpace(sw, bindTool);
                sw.WriteLine("");
                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine(IsPartial ? $"\tpublic partial class {CodeName}" : $"\tpublic class {CodeName}");

                sw.WriteLine("\t{");
                sw.WriteLine("");


                foreach (ComponentAutoBindTool.BindData data in bindTool.BindDatas)
                {
                    sw.WriteLine($"\t\tprivate {data.BindCom.GetType().Name} m_{data.Name};");
                }

                if (m_IsGenAutoBindCodeProperty)
                {
                    foreach (ComponentAutoBindTool.BindData data in bindTool.BindDatas)
                    {
                        sw.WriteLine($"\t\tpublic {data.BindCom.GetType().Name} {data.Name}=>m_{data.Name};");
                    }
                }

                sw.WriteLine("");

                sw.WriteLine("\t\tpublic void GetBindComponents(ComponentAutoBindTool autoBindTool)");
                sw.WriteLine("\t\t{");

                //根据索引获取

                for (int i = 0; i < bindTool.BindDatas.Count; i++)
                {
                    ComponentAutoBindTool.BindData data = bindTool.BindDatas[i];
                    string filedName = $"m_{data.Name}";
                    sw.WriteLine(
                        $"\t\t\t{filedName} = autoBindTool.GetBindComponent<{data.BindCom.GetType().Name}>({i});");
                }


                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }

            if (IsHotfix)
            {
                UpdateHotfixCompile();
            }
        }

        public override bool GenCode()
        {
            if (m_GameObjectRecordableList.GameObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "请选择包含ComponentBindTool的游戏物体", "OK");
                return false;
            }
            for (int i = 0; i < m_GameObjectRecordableList.GameObjects.Count; i++)
            {
                SetCodeName(m_GameObjectRecordableList.GameObjects[i].name);
                GenCode(m_GameObjectRecordableList.GameObjects[i].GetComponent<ComponentAutoBindTool>());
            }
            return true;
        }
    }
}