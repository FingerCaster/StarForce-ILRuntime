using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GameFramework;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace UGFExtensions.Editor
{
    [CodeGenerator]
    public class UIFormCodeGenerator : CodeGeneratorBase
    {
        private bool m_IsGenAutoBindCode = false;
        private bool m_IsGenMainCode = false;
        
        private readonly GameObjectRecordableList m_GameObjectRecordableList;

        public override void Draw()
        {
            m_GameObjectRecordableList.HandleGameObjectsListUI();
            IsHotfix = EditorGUILayout.ToggleLeft("热更层代码", IsHotfix);
            m_IsGenMainCode = EditorGUILayout.ToggleLeft("生成主体代码", m_IsGenMainCode);
            m_IsGenAutoBindCode = EditorGUILayout.ToggleLeft("生成自动绑定组件代码", m_IsGenAutoBindCode);
            EditorGUILayout.LabelField("自动生成代码路径", GetCodeFolder());
        }
        public override bool GenCode()
        {
            if (m_GameObjectRecordableList.GameObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "请选择界面的游戏物体", "OK");
                return false;
            }

            for (int i = 0; i < m_GameObjectRecordableList.GameObjects.Count; i++)
            {
                if (m_IsGenMainCode)
                {
                    SetCodeName(m_GameObjectRecordableList.GameObjects[i].name);
                    GenCode(m_GameObjectRecordableList.GameObjects[i]);
                }

                if (m_IsGenAutoBindCode)
                {
                    BindComponentCodeGenerator generator = new BindComponentCodeGenerator(
                        MainCodeFolder + "/BindComponents",
                        HotfixCodeFolder + "/BindComponents", IsHotfix);
                    generator.SetCodeName(m_GameObjectRecordableList.GameObjects[i].name);
                    generator.IsPartial = true;
                    generator.GenCode(m_GameObjectRecordableList.GameObjects[i].GetOrAddComponent<ComponentAutoBindTool>());
                }
            }

            return true;
        }

        void GenCode(GameObject go)
        {
            string codePath = GetCodePath();
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
            {
                HotfixUGuiForm form = go.GetOrAddComponent<HotfixUGuiForm>();
                FieldInfo hotfixUGuiFormName = typeof(HotfixUGuiForm).GetField("m_HotfixUGuiFormName",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                hotfixUGuiFormName?.SetValue(form, go.name);
                PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(go);
                if (status == PrefabInstanceStatus.Connected)
                {
                    PrefabUtility.ApplyPrefabInstance(go, InteractionMode.AutomatedAction);
                }
            }
            else
            {
                HotfixUGuiForm form = prefabStage.prefabContentsRoot.GetOrAddComponent<HotfixUGuiForm>();
                FieldInfo hotfixUGuiFormName = typeof(HotfixUGuiForm).GetField("m_HotfixUGuiFormName",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                hotfixUGuiFormName?.SetValue(form, prefabStage.prefabContentsRoot.name);
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }

            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }

            using (StreamWriter sw = new StreamWriter(codePath))
            {
                sw.WriteLine("using System.Collections;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("");
                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {CodeName} : UGuiForm");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //OnInit
                sw.WriteLine($"\t\tprotected override void OnInit(object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnInit(userdata);");
                sw.WriteLine("");
                if (m_IsGenAutoBindCode)
                {
                    sw.WriteLine(
                        $"\t\t\tComponentAutoBindTool autoBindTool = gameObject.GetComponent<ComponentAutoBindTool>();");
                    sw.WriteLine($"\t\t\tGetBindComponents(autoBindTool);");
                }

                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }

            if (IsHotfix)
            {
                UpdateHotfixCompile();
            }
        }


        public UIFormCodeGenerator() : base("Assets/Scripts/UI/Custom",
            "UI/Custom")
        {
            m_GameObjectRecordableList = new GameObjectRecordableList();
        }

    }
}