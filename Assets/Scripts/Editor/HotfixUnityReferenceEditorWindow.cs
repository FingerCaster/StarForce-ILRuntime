using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using GameFramework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UGFExtensions.Editor
{
    public class HotfixUnityReferencesEditorWindow : EditorWindow
    {
        [Serializable]
        public class Reference
        {
            public Reference(string path)
            {
                Assembly assembly = Assembly.LoadFile(path);
                m_Assembly = assembly;
                m_FullName = assembly.FullName;
                m_Name = assembly.GetName().Name;
                Dirty = false;
                m_IsActive = false;
                m_Path = path;
            }

            private string m_Name;
            private string m_FullName;
            private bool m_Dirty;
            private bool m_IsActive;
            private string m_Path;

            public string Path
            {
                get => m_Path;
                private set => m_Path = value;
            }

            private Assembly m_Assembly;

            public Assembly Assembly
            {
                get => m_Assembly;
                private set => m_Assembly = value;
            }

            public bool IsActive
            {
                get => m_IsActive;
                set
                {
                    if (value != m_IsActive)
                    {
                        Dirty = true;
                    }
                    m_IsActive = value;
                }
            }

            public string Name
            {
                get => m_Name;
                private set => m_Name = value;
            }

            public string FullName
            {
                get => m_FullName;
                set => m_FullName = value;
            }

            public bool Dirty
            {
                get => m_Dirty;
                set => m_Dirty = value;
            }

            
        }


        private List<Reference> m_References;
        private ReorderableList m_ReorderableList;
        private Vector2 m_ScrollViewPos;
        private readonly XmlDocument m_ProjectDoc = new XmlDocument();
        [MenuItem("Tools/Hotfix Unity References")]
        private static void ShowWindow()
        {
            var window = GetWindow<HotfixUnityReferencesEditorWindow>();
            window.titleContent = new GUIContent("Hotfix Unity References");
            window.minSize = new Vector2(500f, 300f);
            window.Show();
        }

        private void OnEnable()
        {
            m_ProjectDoc.Load(m_HotfixProjectPath);
            m_References = new List<Reference>();
            InitUnityReference();
            SetHotfixUsed();
            m_ReorderableList = new ReorderableList(m_References, typeof(Reference),true, true, true, true)
            {
                displayAdd = false,
                displayRemove = false,
                drawElementCallback = DrawReference,
                elementHeight = EditorGUIUtility.singleLineHeight,
                headerHeight = 0f,
                draggable = false,
                showDefaultBackground = false,
            };
        }

        private string m_HotfixProjectPath = "Assets/Scripts/Hotfix~/Hotfix.csproj";
        private string m_HotfixUnityDllPath = "Assets/Scripts/Hotfix~/Tools/UnityDll";
        private string m_HotfixUnityDllRelativePath = "Tools/UnityDll";
        private void SetHotfixUsed()
        {
            XmlNodeList xnl = m_ProjectDoc.ChildNodes[1].ChildNodes;
            XmlNode itemGroup = null;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "ItemGroup") continue;
                if (xn.ChildNodes.Count <= 0) continue;
                if (xn.ChildNodes[0].Name != "Reference") continue;
                itemGroup = xn;
                break;
            }

            if (itemGroup != null)
            {
                var references = itemGroup.ChildNodes;
                foreach (XmlNode xmlNode in references)
                {
                    if (xmlNode.Attributes == null) continue;
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name == "Include")
                        {
                            Reference reference = m_References.Find(_ => _.FullName == attribute.InnerText);
                            if (reference!= null)
                            {
                                reference.IsActive = true;
                            }
                        }
                    }
                }
            }
            m_References.Sort((x, y) => y.IsActive.CompareTo(x.IsActive));

        }

        /// <summary>
        /// 添加引用
        /// </summary>
        /// <param name="reference">引用</param>
        /// <param name="path">文件相对热更工程的相对路径</param>
        /// <param name="isAdd">是否添加</param>
        private void RefreshReference(Reference reference,string path,bool isAdd)
        {
            XmlNodeList xnl = m_ProjectDoc.ChildNodes[1].ChildNodes;
            XmlNode itemGroup = null;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "ItemGroup") continue;
                if (xn.ChildNodes.Count <= 0) continue;
                if (xn.ChildNodes[0].Name != "Reference") continue;
                itemGroup = xn;
                break;
            }

            if (itemGroup!= null)
            {
                var references = itemGroup.ChildNodes;
                XmlNode currentReference = null;
                foreach (XmlNode xmlNode in references)
                {
                    if (xmlNode.Attributes == null) continue;
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name == "Include")
                        {
                            if (attribute.InnerText == reference.Assembly.FullName)
                            {
                                currentReference = xmlNode;
                                break;
                            }
                        }
                    }
                }

                if (isAdd)
                {
                    if (currentReference != null) return;
                    XmlElement referenceNode = m_ProjectDoc.CreateChildElement(itemGroup,"Reference");
                    XmlAttribute include = m_ProjectDoc.CreateAttribute("Include");
                    include.InnerText = reference.Assembly.FullName;;
                    referenceNode.SetAttributeNode(include);
                    
                    XmlElement hintPath = m_ProjectDoc.CreateChildElement(referenceNode,"HintPath");
                    hintPath.InnerText = path.Replace('/','\\');
                
                    XmlElement privateNode = m_ProjectDoc.CreateChildElement(referenceNode,"Private");
                    privateNode.InnerText = "False";
                    m_ProjectDoc.Save(m_HotfixProjectPath);

                }
                else
                {
                    currentReference?.ParentNode?.RemoveChild(currentReference);
                    m_ProjectDoc.Save(m_HotfixProjectPath);

                }

                
            }
            else
            {
                if (isAdd)
                {
                    itemGroup = m_ProjectDoc.CreateChildElement(m_ProjectDoc.ChildNodes[1],"ItemGroup");
                    XmlElement referenceNode = m_ProjectDoc.CreateChildElement(itemGroup,"Reference");
                    XmlAttribute include = m_ProjectDoc.CreateAttribute("Include");
                    include.InnerText = reference.Assembly.FullName;
                    referenceNode.SetAttributeNode(include);
                
                    XmlElement hintPath = m_ProjectDoc.CreateChildElement(referenceNode,"HintPath");
                    hintPath.InnerText = path.Replace('/','\\');
                
                    XmlElement privateNode = m_ProjectDoc.CreateChildElement(referenceNode,"Private");
                    privateNode.InnerText = "False";
                
                    m_ProjectDoc.Save(m_HotfixProjectPath);
                }
              
            }
        }

        private void DrawReference(Rect rect, int index, bool isactive, bool isfocused)
        {
            Reference reference = m_References[index];
            var width = rect.width;
            rect.width = EditorGUIUtility.labelWidth;
            reference.IsActive = EditorGUI.ToggleLeft(rect, "IsActive",reference.IsActive);
            rect.width = width;
            rect.x = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(rect,reference.Name);
        }

        private void InitUnityReference()
        {
            string path = EditorApplication.applicationContentsPath + "/Managed/UnityEngine";
            string[] dlls = Directory.GetFiles(path, "*.dll");
            foreach (string dllPath in dlls)
            {
             
                Reference reference = new Reference(dllPath);
                m_References.Add(reference);
            }
        }


        private void OnGUI()
        {
            m_ScrollViewPos = EditorGUILayout.BeginScrollView(m_ScrollViewPos);
            m_ReorderableList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            if (IsDirtied())
            {
                if (GUILayout.Button("Refresh References"))
                {
                    foreach (var reference in m_References)
                    {
                        if (reference.Dirty)
                        {
                            RefreshReference(reference, GetHotfixDllRelativePath(reference, reference.IsActive),
                                reference.IsActive);
                            reference.Dirty = false;
                        }
                    }
                    m_References.Sort((x, y) => y.IsActive.CompareTo(x.IsActive));
                }
            }
        }
    
        string GetHotfixDllRelativePath(Reference reference,bool isAdd)
        {
            string dllPath = Path.Combine(m_HotfixUnityDllPath, reference.Name);
            if (File.Exists(dllPath))
            {
                if (!isAdd)
                {
                    File.Delete(dllPath);
                }
            }
            else
            {
                if (isAdd)
                {
                    File.Copy(reference.Path, dllPath);
                }
            }
            return Path.Combine(m_HotfixUnityDllRelativePath, reference.Name);

        }

        private bool IsDirtied()
        {
            foreach (var reference in m_References)
            {
                if (reference.Dirty)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
}