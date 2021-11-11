//
// ILRuntimeCrossBinding.cs
//
// Author:
//       JasonXuDeveloper（傑） <jasonxudeveloper@gmail.com>
//
// Copyright (c) 2020 JEngine
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using Debug = UnityEngine.Debug;

namespace UGFExtensions.Editor
{
    public class ILRuntimeCrossBindingAdapterGenerator : EditorWindow
    {
        private static ILRuntimeCrossBindingAdapterGenerator window;
        private const string OUTPUT_PATH = "Assets/Scripts/Hotfix/ILRuntime/Adaptor";

        [MenuItem("Tools/ILRuntime/Generate Cross bind Adapter", priority = 1001)]
        public static void ShowWindow()
        {
            window = GetWindow<ILRuntimeCrossBindingAdapterGenerator>();
            window.titleContent = new GUIContent("Generate Cross bind Adapter");
            window.minSize = new Vector2(300, 150);
            window.Show();
        }

        private string _assembly = "Assembly-CSharp";
        private string _class;
        private string _namespace = "UGFExtensions";

        private void OnGUI()
        {
            //绘制标题
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("ILRuntime适配器生成");
            GUI.skin.label.fontSize = 18;
            GUILayout.Label("ILRuntime Adapter Generator");

            //介绍
            EditorGUILayout.HelpBox("本地工程类（没生成asmdef的），Assembly一栏不需要改，Class name一栏写类名（有命名空间带上）；\n" +
                                    "有生成asmdef的工程类，Assembly一栏写asmdef里写的名字，Class name一栏写类名（有命名空间带上）；\n" +
                                    "最后的Namespace是生成的适配器的命名空间", MessageType.Info);

            //程序集
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            EditorGUILayout.LabelField("Assembly 类的程序集");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            _assembly = EditorGUILayout.TextField("", _assembly);
            GUILayout.Space(25);
            GUILayout.EndHorizontal();

            //类名
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            _class = EditorGUILayout.TextField("Class name 类名", _class);
            GUILayout.Space(25);
            GUILayout.EndHorizontal();

            //命名空间
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            EditorGUILayout.LabelField("Namespace for generated adapter 生成适配器的命名空间");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            _namespace = EditorGUILayout.TextField("", _namespace);
            GUILayout.Space(25);
            GUILayout.EndHorizontal();

            //生成
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            if (GUILayout.Button("Generate 生成"))
            {
                GenAdapter();
            }

            GUILayout.Space(25);
            GUILayout.EndHorizontal();
        }

        private void GenAdapter()
        {
            //获取主工程DLL的类
            System.Type t = System.Type.GetType(_class);
            if (t == null)
            {
                t = Assembly.Load(_assembly).GetType(_class);
            }

            //判断空
            if (t == null)
            {
                EditorUtility.DisplayDialog("Error", $"Invalid Class {_class}!\r\n{_class}类不存在！", "Ok");
                return;
            }
            string className = t.FullName.Replace(".", "_");

            if (!Directory.Exists(OUTPUT_PATH))
            {
                Directory.CreateDirectory(OUTPUT_PATH);
            }

            //如果有先删除
            if (File.Exists($"{OUTPUT_PATH}/{className}Adapter.cs"))
            {
                File.Delete($"{OUTPUT_PATH}/{className}Adapter.cs");
                if (File.Exists($"{OUTPUT_PATH}/{className}Adapter.cs.meta"))
                {
                    File.Delete($"{OUTPUT_PATH}/{className}Adapter.cs.meta");
                }

                AssetDatabase.Refresh();
            }

            //生成
            FileStream stream = new FileStream($"{OUTPUT_PATH}/{className}Adapter.cs", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);
            Stopwatch watch = new Stopwatch();
            sw.WriteLine(
                CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(t,
                    _namespace));
            watch.Stop();
            Log.Info($"Generated {OUTPUT_PATH}/{className}Adapter.cs in: " +
                     watch.ElapsedMilliseconds + " ms.");
            Log.Info($"Please insert " +
                     $"'appdomain.RegisterCrossBindingAdaptor(new {className}Adapter());' " +
                     $"into 'Register(AppDomain appdomain)' method in " +
                     $"Scripts/Helpers/RegisterCrossBindingAdaptorHelper.cs");

            sw.Dispose();


            AssetDatabase.Refresh();
        }
    }
}