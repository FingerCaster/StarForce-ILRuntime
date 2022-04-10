using System;
using System.IO;

namespace UGFExtensions.Editor
{
    public class EntityShowCodeGenerator : CodeGeneratorBase
    {
        private string m_EntityName;

        public EntityShowCodeGenerator(string mainCodeFolder, string hotfixCodeFolder, bool isHotfix, string entityName) :
            base(mainCodeFolder, hotfixCodeFolder)
        {
            IsHotfix = isHotfix;
            m_EntityName = entityName;
        }

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }

        public override bool GenCode()
        {
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            
            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }

            using (StreamWriter sw = new StreamWriter(GetCodePath()))
            {
                sw.WriteLine("using System.Threading.Tasks;");
                sw.WriteLine("using UnityGameFramework.Runtime;");
                sw.WriteLine("using UGFExtensions.Await;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");


                //类名
                sw.WriteLine("\tpublic static partial class ShowEntityExtension");
                sw.WriteLine("\t{");

                //显示实体的方法
                sw.WriteLine(
                    $"\t\tpublic static void Show{m_EntityName}(this EntityComponent entityComponent,{m_EntityName}Data data)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tentityComponent.ShowEntity(typeof({m_EntityName}Logic), 0, data);");
                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                //显示实体的可等待扩展
                sw.WriteLine(
                    $"\t\tpublic static async Task<{m_EntityName}Logic> Show{m_EntityName}Async(this EntityComponent entityComponent,{m_EntityName}Data data)");
                sw.WriteLine("\t\t{");
                sw.WriteLine(
                    $"\t\t\tEntity entity = await entityComponent.ShowEntityAsync(typeof({m_EntityName}Logic), 0, data);");
                
                sw.WriteLine($"\t\t\treturn entity.Logic as {m_EntityName}Logic;");

                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }
            return true;
        }
    }
}