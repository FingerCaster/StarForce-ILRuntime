using System.Collections.Generic;
using UnityEngine;

namespace UGFExtensions
{
    public class AutoBindRuleHelper : IAutoBindRuleHelper
    {
        /// <summary>
        /// 命名前缀与类型的映射
        /// </summary>
        private Dictionary<string, string> m_PrefixesDict = new Dictionary<string, string>()
        {
            {"Trans","Transform" },
            {"OldAnim","Animation"}, 
            {"NewAnim","Animator"},

            {"Rect","RectTransform"},
            {"Canvas","Canvas"},
            {"Group","CanvasGroup"},
            {"VGroup","VerticalLayoutGroup"},
            {"HGroup","HorizontalLayoutGroup"},
            {"GGroup","GridLayoutGroup"},
            {"TGroup","ToggleGroup"},

            {"Btn","Button"},
            {"Img","Image"},
            {"RImg","RawImage"},
            {"Txt","Text"},
            {"Input","InputField"},
            {"Slider","Slider"},
            {"Mask","Mask"},
            {"Mask2D","RectMask2D"},
            {"Tog","Toggle"},
            {"Sbar","Scrollbar"},
            {"SRect","ScrollRect"},
            {"Drop","Dropdown"},
        };
        public bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames)
        {
            throw new System.NotImplementedException();
        }
    }
}