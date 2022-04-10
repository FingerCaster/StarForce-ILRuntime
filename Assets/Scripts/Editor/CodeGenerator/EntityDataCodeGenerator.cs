using System;
using System.IO;

namespace UGFExtensions.Editor
{
    public class EntityDataCodeGenerator : CodeGeneratorBase
    {
        public EntityDataCodeGenerator(string mainCodeFolder, string hotfixCodeFolder, bool isHotfix) : base(
            mainCodeFolder, hotfixCodeFolder)
        {
            IsHotfix = isHotfix;
        }

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }

        public override bool GenCode()
        {
            string nameSpace = IsHotfix ? "UGFExtensions.Hotfix" : "UGFExtensions";
            string codePath = GetCodePath();
            string dataBaseClass = "EntityData";

            if (!Directory.Exists(GetCodeFolder()))
            {
                Directory.CreateDirectory(GetCodeFolder());
            }

            using (StreamWriter sw = new StreamWriter(codePath))
            {
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic class {CodeName} : {dataBaseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //构造方法
                sw.WriteLine($"\t\tpublic {CodeName}()");
                sw.WriteLine("\t\t{");

                sw.WriteLine("\t\t}");
                sw.WriteLine("");


                //Fill方法
                sw.WriteLine($"\t\tpublic {CodeName} Fill(int typeId)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tFill(GameEntry.Entity.GenerateSerialId(),typeId);");
                sw.WriteLine("\t\t\treturn this;");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //Clear方法
                sw.WriteLine("\t\tpublic override void Clear()");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.Clear();");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
            return true;
        }
    }
}