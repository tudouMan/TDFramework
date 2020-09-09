using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDFramework.UI
{
    public enum BindType
    {
        /// <summary>
        /// 自动判断类别 
        /// </summary>
        Default,

        /// <summary>
        /// 手动绑定类型
        /// </summary>
        Custom,

        ///// <summary>
        ///// 自定义类型脚本  TODO
        ///// </summary>
        //Component,

    }


    public class UIBind:MonoBehaviour
    {
        [HideInInspector]public BindType BindType=BindType.Default;
        [HideInInspector] public string SelectTypeName;


        public string ComponentName
        {
            get
            {
                if (BindType == BindType.Default)
                    return DefaultName();
                else
                    return SelectTypeName;
            }
        }

        public string DefaultName()
        {
            if (GetComponent<TMPro.TextMeshProUGUI>()) return "TMPro.TextMeshProUGUI";
            if (GetComponent<TMPro.TMP_Dropdown>()) return "TMPro.TMP_Dropdown";
            if (GetComponent<TMPro.TMP_InputField>()) return "TMPro.TMP_InputField";
            if (GetComponent<TMPro.TMP_InputField>()) return "TMPro.TMP_InputField";
            if (GetComponent<UnityEngine.Canvas>()) return "UnityEngine.Canvas";
            if (GetComponent<UnityEngine.CanvasGroup>()) return "UnityEngine.CanvasGroup";
            if (GetComponent<UnityEngine.UI.RawImage>()) return "UnityEngine.UI.RawImage";
            if (GetComponent<UnityEngine.UI.Button>()) return "UnityEngine.UI.Button";
            if (GetComponent<UnityEngine.UI.Image>()) return "UnityEngine.UI.Image";
            if (GetComponent<UnityEngine.UI.Text>()) return "UnityEngine.UI.Text";
            if (GetComponent<UnityEngine.UI.ToggleGroup>()) return "UnityEngine.UI.ToggleGroup";
            if (GetComponent<UnityEngine.UI.Toggle>()) return "UnityEngine.UI.Toggle";
            if (GetComponent<UnityEngine.UI.Slider>()) return "UnityEngine.UI.Slider";
            if (GetComponent<UnityEngine.UI.Scrollbar>()) return "UnityEngine.UI.Scrollbar";
            if (GetComponent<UnityEngine.UI.Dropdown>()) return "UnityEngine.UI.Dropdown";
            if (GetComponent<UnityEngine.UI.ScrollRect>()) return "UnityEngine.UI.ScrollRect";
            if (GetComponent<UnityEngine.RectTransform>()) return "UnityEngine.RectTransform";
            if (GetComponent<UnityEngine.GameObject>()) return "UnityEngine.GameObject";
            if (GetComponent<UnityEngine.Transform>()) return "UnityEngine.Transform";
            if (GetComponent<UnityEngine.CanvasRenderer>()) return "UnityEngine.CanvasRenderer";
            return string.Empty;
        }
    }


   
}
