using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UGFExtensions.Editor
{
    [CustomEditor(typeof(Test))]
    public class TestEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Components;
        private SerializedProperty m_GameObjects;
        private ReorderableList m_ReorderableList;

        private void OnEnable()
        {
            m_Components = serializedObject.FindProperty("Components");
            m_GameObjects = serializedObject.FindProperty("GameObjects");
            m_ReorderableList = new ReorderableList(serializedObject, m_Components)
            {
                drawElementCallback = OnDrawElement,
                onAddCallback = OnAdd,
            };
        }

        private void OnAdd(ReorderableList list)
        {
            m_Components.InsertArrayElementAtIndex(m_Components.arraySize);
            m_GameObjects.InsertArrayElementAtIndex(m_GameObjects.arraySize);
        }

        private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused)
        {
            var gameObject = m_GameObjects.GetArrayElementAtIndex(index);
            rect.width /= 2;
            gameObject.objectReferenceValue = EditorGUI.ObjectField(rect, gameObject.objectReferenceValue, typeof(GameObject),true);
            GameObject go = (GameObject)gameObject.objectReferenceValue;
            if (go == null)
            {
                return;
            }
            Component[] components = go.GetComponents<Component>();
            string[] componentNames = components.Select(_ => _.GetType().Name).ToArray();
            rect.x = rect.width+50;
            rect.width -= 10;
            int comIndex = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (m_Components.GetArrayElementAtIndex(index).objectReferenceValue == components[i])
                {
                    comIndex = i;
                    break;
                }
            }
            int newComIndex = EditorGUI.Popup(rect, comIndex, componentNames);
            if (index != newComIndex)
            {
                m_Components.GetArrayElementAtIndex(index).objectReferenceValue = components[newComIndex];
            }
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_ReorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}