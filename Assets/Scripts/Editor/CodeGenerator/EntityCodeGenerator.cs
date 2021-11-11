using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UGFExtensions.Editor
{
    [CodeGenerator]
    public class EntityCodeGenerator : CodeGeneratorBase
    {
        private readonly GameObjectRecordableList m_GameObjectRecordableList;
        private bool m_IsGenMainCode = false;
        private bool m_IsGenAutoBindCode = false;
        private bool m_IsGenEntityDataCode = false;
        private bool m_IsGenShowEntityCode = false;
        private string m_EntityName;

        public EntityCodeGenerator() : base("Assets/Scripts/Entity/Custom/EntityLogic", "Entity/Custom/EntityLogic")
        {
            m_GameObjectRecordableList = new GameObjectRecordableList();
        }


        public override void Draw()
        {
            m_GameObjectRecordableList.HandleGameObjectsListUI();
            IsHotfix = EditorGUILayout.ToggleLeft("热更层代码", IsHotfix);     
            m_IsGenMainCode = EditorGUILayout.ToggleLeft("生成主体代码", m_IsGenMainCode);
            m_IsGenAutoBindCode = EditorGUILayout.ToggleLeft("生成自动绑定组件代码", m_IsGenAutoBindCode);
            m_IsGenEntityDataCode = EditorGUILayout.ToggleLeft("生成实体数据代码", m_IsGenEntityDataCode);
            m_IsGenShowEntityCode = EditorGUILayout.ToggleLeft("生成快捷显示实体代码", m_IsGenShowEntityCode);
            EditorGUILayout.LabelField("自动生成代码路径", GetCodeFolder());
        }

        public override bool GenCode()
        {
            if (m_GameObjectRecordableList.GameObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "请选择实体的游戏物体", "OK");
                return false;
            }
            foreach (GameObject go in m_GameObjectRecordableList.GameObjects)
            {
                m_EntityName = go.name;

                if (m_IsGenMainCode)
                {
                    SetCodeName(m_EntityName+"Logic");
                    GenEntityLogicCode();
                }
                if (m_IsGenEntityDataCode)
                {
                    EntityDataCodeGenerator codeGenerator =
                       new EntityDataCodeGenerator("Assets/Scripts/Entity/Custom/EntityData",
                           "Entity/Custom/EntityData", IsHotfix);
                   codeGenerator.SetCodeName(m_EntityName + "Data");
                   codeGenerator.GenCode();
                }

                if (m_IsGenAutoBindCode)
                {
                    BindComponentCodeGenerator generator = new BindComponentCodeGenerator(MainCodeFolder + "/BindComponents",
                        HotfixCodeFolder + "/BindComponents",IsHotfix);
                    generator.SetCodeName(m_EntityName+"Logic");
                    generator.IsPartial = true;
                    generator.GenCode(go.GetOrAddComponent<ComponentAutoBindTool>());
                }

                if (m_IsGenShowEntityCode)
                {
                   EntityShowCodeGenerator codeGenerator =
                       new EntityShowCodeGenerator("Assets/Scripts/Entity/Custom/ShowEntityExtension",
                           "Entity/Custom/ShowEntityExtension", IsHotfix,m_EntityName);
                   codeGenerator.SetCodeName("ShowEntityExtension");
                   codeGenerator.SetSuffix($".Show{m_EntityName}.cs");
                   codeGenerator.GenCode();
                }

                ApplyPrefab(go);
            }

            return true;
        }

        private void ApplyPrefab(GameObject go)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
            {
                PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(go);
                if (status == PrefabInstanceStatus.Connected)
                {
                    PrefabUtility.ApplyPrefabInstance(go, InteractionMode.AutomatedAction);
                }
            }
            else
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        private void GenEntityLogicCode()
        {
            string codePath = GetCodePath();
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            string entityDataName = m_EntityName + "Data";
            
            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }
          

            using (StreamWriter sw = new StreamWriter(codePath))
            {
                sw.WriteLine("using System.Collections;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using UnityEngine;");
                if (!IsHotfix)
                {
                    sw.WriteLine("using GameFramework;");
                }

                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {CodeName} : EntityLogic");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //定义实体数据字段
                sw.WriteLine($"\t\tprivate {entityDataName} m_{entityDataName};");
                sw.WriteLine("");

                //OnInit方法 获取对象引用
                sw.WriteLine($"\t\tprotected override void OnInit(object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnInit(userdata);");
                sw.WriteLine("");
                if (m_IsGenAutoBindCode)
                {
                    sw.WriteLine($"\t\t\tComponentAutoBindTool autoBindTool = gameObject.GetComponent<ComponentAutoBindTool>();");
                    sw.WriteLine($"\t\t\tGetBindComponents(autoBindTool);");
                }

                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnShow方法 获取实体数据
                sw.WriteLine($"\t\tprotected override void OnShow(object userData)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.OnShow(userData);");
                sw.WriteLine("");

                sw.WriteLine($"\t\t\tm_{entityDataName} = ({entityDataName})userData;");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnHide方法 归还实体数据引用
                sw.WriteLine($"\t\tprotected override void OnHide(bool isShutDown,object userData)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.OnHide(isShutDown,userData);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
            if (IsHotfix)
            {
                UpdateHotfixCompile();
            }
        }
    }
}