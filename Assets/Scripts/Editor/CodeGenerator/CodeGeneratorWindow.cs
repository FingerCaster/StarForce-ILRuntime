using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UGFExtensions.Editor
{
    public class CodeGeneratorWindow : EditorWindow
    {
        private List<CodeGeneratorBase> m_CodeGenerators;
        private string[]  m_CodeGeneratorNames;
        private int m_Current;
        [MenuItem("Tools/Code Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CodeGeneratorWindow>(true,"代码生成器");
            window.minSize = new Vector2(600f, 300f);
            window.Show();
        }

        private void OnEnable()
        {
            m_CodeGenerators = Type.GetEditorTypes(typeof(CodeGeneratorBase)).Where(_ =>
            {
                CodeGeneratorAttribute attribute = (CodeGeneratorAttribute)_
                    .GetCustomAttributes(typeof(CodeGeneratorAttribute), false).FirstOrDefault();
                return attribute != null && attribute.IsShow;
            }).Select(_=> (CodeGeneratorBase)Activator.CreateInstance(_)).ToList();
            m_CodeGeneratorNames = m_CodeGenerators.Select(_ => _.GetType().Name.Replace("CodeGenerator", "")).ToArray();
        }

        private void OnGUI()
        {
            m_Current = EditorGUILayout.Popup("生成代码类型:", m_Current, m_CodeGeneratorNames);
            m_CodeGenerators[m_Current].Draw();
         
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            Rect rect = EditorGUILayout.GetControlRect();
            if (GUI.Button(rect, "生成代码"))
            {
                if (m_CodeGenerators[m_Current].GenCode())
                {
                    EditorUtility.DisplayDialog("", "生成代码成功", "OK");
                }
            }
        }
    }
}