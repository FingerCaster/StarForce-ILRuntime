using UnityEngine;

namespace UGFExtensions
{
    public static class UnityExtensions
    {
        public static T AddHotfixMonoBehaviour<T>(this GameObject go, string hotfixFullTypeName) where T : MonoBehaviour
        {
            return GameEntry.Hotfix.AddHotfixMonoBehaviour<T>(go, hotfixFullTypeName);
        }
    }
}