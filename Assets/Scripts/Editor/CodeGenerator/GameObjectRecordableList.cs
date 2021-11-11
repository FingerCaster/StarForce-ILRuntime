using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UGFExtensions.Editor
{
    public class GameObjectRecordableList
    {
        private List<GameObject> m_GameObjects;
        private readonly ReorderableList m_ReorderableList;
        private bool m_IsSelect = false;
        private readonly int m_SelectorHash = typeof(GameObjectRecordableList).GetHashCode();

        public List<GameObject> GameObjects
        {
            get => m_GameObjects;
            private set => m_GameObjects = value;
        }

        public void HandleGameObjectsListUI()
        {
            var currentEvent = Event.current;
            var usedEvent = false;
            Rect rect = EditorGUILayout.GetControlRect();
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
                    {
                        // Check each single object, so we can add multiple objects in a single drag.
                        var didAcceptDrag = false;
                        var references = DragAndDrop.objectReferences;
                        foreach (var obj in references)
                        {
                            if (obj is GameObject go)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                if (currentEvent.type == EventType.DragPerform)
                                {
                                    m_GameObjects.Add(go);
                                    didAcceptDrag = true;
                                    DragAndDrop.activeControlID = 0;
                                }
                                else
                                    DragAndDrop.activeControlID = controlID;
                            }
                        }

                        if (didAcceptDrag)
                        {
                            GUI.changed = true;
                            DragAndDrop.AcceptDrag();
                            usedEvent = true;
                        }
                    }

                    break;
                case EventType.ExecuteCommand:
                    if (currentEvent.commandName == "ObjectSelectorClosed" &&
                        EditorGUIUtility.GetObjectPickerControlID() == m_SelectorHash)
                    {
                        var obj = EditorGUIUtility.GetObjectPickerObject();
                        if (obj is GameObject go)
                        {
                            m_GameObjects.Add(go);
                        }
                    }

                    usedEvent = true;
                    break;
            }

            if (usedEvent)
                currentEvent.Use();
            m_IsSelect = EditorGUI.Foldout(rect, m_IsSelect, "GameObjects:");
            if (m_IsSelect)
            {
                EditorGUI.indentLevel++;
                m_ReorderableList.DoLayoutList();
                EditorGUI.indentLevel--;
            }
        }

        public GameObjectRecordableList()
        {
            m_GameObjects = new List<GameObject>();
            m_ReorderableList = new ReorderableList(m_GameObjects, typeof(GameObject), true, true, true, true)
            {
                onAddCallback = AddGameObject,
                onRemoveCallback = RemoveObject,
                drawElementCallback = DrawPackableElement,
                elementHeight = EditorGUIUtility.singleLineHeight,
                headerHeight = 0f,
            };
        }

        void AddGameObject(ReorderableList list)
        {
            EditorGUIUtility.ShowObjectPicker<GameObject>(null, true, "t:GameObject", m_SelectorHash);
        }

        void RemoveObject(ReorderableList list)
        {
            var index = list.index;
            if (index != -1)
            {
                m_GameObjects.RemoveAt(index);
                m_ReorderableList.index = m_GameObjects.Count - 1;
            }
        }

        void DrawPackableElement(Rect rect, int index, bool selected, bool focused)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive, rect);
            EditorGUI.BeginChangeCheck();
            GameObject changedObject =
                (GameObject)EditorGUI.ObjectField(rect, m_GameObjects[index], typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {
                m_GameObjects[index] = changedObject;
            }

            if (GUIUtility.keyboardControl == controlID && !selected)
                m_ReorderableList.index = index;
        }
    }
}