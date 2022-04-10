using System;
using UGFExtensions.Hotfix;
using UnityEditor;
using UnityGameFramework.Editor;

namespace UGFExtensions.Editor
{
    [CustomEditor(typeof(HotfixComponent))]
    public class HotfixComponentInspector : GameFrameworkInspector
    {
        private HelperInfo<HotfixHelperBase> m_HotfixHelperInfo = new HelperInfo<HotfixHelperBase>("Hotfix");
        private SerializedProperty m_HelperTypeName;
        private string m_LastHelperTypeName;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_HotfixHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
            if (m_HelperTypeName.stringValue != m_LastHelperTypeName)
            {
                EditorChangedProcessor();
                m_LastHelperTypeName = m_HelperTypeName.stringValue;
            }
            Repaint();
        }

        private void EditorChangedProcessor()
        {
            string[] sps = m_HelperTypeName.stringValue.Split('.');
            string helperName = sps[sps.Length - 1].Replace("Helper", "");
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_HotfixHelperInfo.Init(serializedObject);
            m_HelperTypeName = serializedObject.FindProperty("m_HotfixHelperTypeName");
            m_LastHelperTypeName = String.Empty;
            RefreshTypeNames();
        }
        private void RefreshTypeNames()
        {
            m_HotfixHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}