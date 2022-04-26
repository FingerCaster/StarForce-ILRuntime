using UnityEditor;
using UnityEngine;

namespace UGFExtensions
{
    public class OnGenerateCSProjectProcessor : AssetPostprocessor
    {
        public static string OnGeneratedCSProject(string path, string content)
        {
            if (path.EndsWith("Assembly-CSharp.csproj"))
            {
                return content.Replace("<OutputPath>Temp\\Bin\\Debug\\Assembly-CSharp\\</OutputPath>", "<OutputPath>Temp\\Bin\\Debug\\</OutputPath>");
            }
            return content;
        }
    }
}