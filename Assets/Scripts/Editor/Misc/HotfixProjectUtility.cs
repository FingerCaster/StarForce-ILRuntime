using System.Xml;
using GameFramework;
using UnityEditor;

namespace UGFExtensions.Editor
{
    public static class HotfixProjectUtility
    {
        private static string HotfixProjectPath = "Assets/Scripts/Hotfix~/Hotfix.csproj";
        
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="path">文件相对热更工程的相对路径</param>
        public static void AddCompileItem(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HotfixProjectPath);
            XmlNodeList xnl = doc.ChildNodes[1].ChildNodes;
            XmlNode itemGroup = null;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "ItemGroup") continue;
                if (xn.ChildNodes.Count <= 0) continue;
                if (xn.ChildNodes[0].Name != "Compile") continue;
                itemGroup = xn;
                break;
            }

            if (itemGroup!= null)
            {
                var compiles = itemGroup.ChildNodes;
                bool isExist = false;
                foreach (XmlNode compile in compiles)
                {
                    if (compile.Attributes == null) continue;
                    foreach (XmlAttribute attribute in compile.Attributes)
                    {
                        if (Utility.Path.GetRegularPath(attribute.InnerText) == Utility.Path.GetRegularPath(path))
                        {
                            isExist = true;
                            break;
                        }
                    }
                }

                if (!isExist)
                {
                    XmlElement xelKey = doc.CreateChildElement(itemGroup,"Compile");
                    XmlAttribute xelType = doc.CreateAttribute("Include");
                    xelType.InnerText = path.Replace('/','\\');;
                    xelKey.SetAttributeNode(xelType);
                    doc.Save(HotfixProjectPath);
                }
            }
            else
            {
                XmlElement xmlElement = doc.CreateChildElement(doc.ChildNodes[1],"ItemGroup");
                XmlElement xelKey = doc.CreateChildElement(xmlElement,"Compile");
                XmlAttribute xelType = doc.CreateAttribute("Include");
                xelType.InnerText = path.Replace('/','\\');
                xelKey.SetAttributeNode(xelType);
                doc.Save(HotfixProjectPath);
            }
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="path">文件相对热更工程的相对路径</param>
        public static void RemoveCompileItem(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HotfixProjectPath);
            XmlNodeList xnl = doc.ChildNodes[1].ChildNodes;
            XmlNode itemGroup = null;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "ItemGroup") continue;
                if (xn.ChildNodes.Count <= 0) continue;
                if (xn.ChildNodes[0].Name != "Compile") continue;
                itemGroup = xn;
                break;
            }

            if (itemGroup!= null)
            {
                var compiles = itemGroup.ChildNodes;
                foreach (XmlNode compile in compiles)
                {
                    if (compile.Attributes == null) continue;
                    foreach (XmlAttribute attribute in compile.Attributes)
                    {
                        if (Utility.Path.GetRegularPath(attribute.InnerText) == Utility.Path.GetRegularPath(path))
                        {
                            itemGroup.RemoveChild(compile);
                            doc.Save(HotfixProjectPath);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 修改热更编译后处理事件
        /// </summary>
        /// <param name="content">命令语句</param>
        public static void ChangePostBuildEvent(string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HotfixProjectPath);
            XmlNodeList xnl = doc.ChildNodes[1].ChildNodes;
            XmlNode postBuildEvent = null;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "PropertyGroup") continue;
                foreach ( XmlNode childNode in xn.ChildNodes)
                {
                    if (childNode.Name != "PostBuildEvent") continue;
                    postBuildEvent = childNode;
                    break;
                }

                if (postBuildEvent!= null)
                {
                    break;
                }
            }

            if (postBuildEvent!= null)
            {
                if (postBuildEvent.InnerText == content)
                {
                    return;
                }
                postBuildEvent.InnerText = content;
                doc.Save(HotfixProjectPath);
            }
            else
            {
                XmlElement xmlElement = doc.CreateChildElement(doc.ChildNodes[1],"PropertyGroup");
                XmlElement xelKey = doc.CreateChildElement(xmlElement,"PostBuildEvent");
                xelKey.InnerText = content;
                doc.Save(HotfixProjectPath);
            }
        }
        public static XmlElement CreateChildElement(this XmlDocument document,XmlNode parent,string qualifiedName)
        {
            XmlElement xmlElement = document.CreateElement(qualifiedName,parent.NamespaceURI);
            parent.AppendChild(xmlElement);
            return xmlElement;
        }
        

        
    }
}