using System;
using UGFExtensions.SpriteCollection.Editor;
using UnityEditor;
using UnityEngine;

namespace UGFExtensions.SpriteCollection.Editor
{
    [InitializeOnLoadAttribute]
    public static class InitializeOnLoad
    {
        static InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    SpriteCollectionUtility.RefreshSpriteCollection();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
    }
}