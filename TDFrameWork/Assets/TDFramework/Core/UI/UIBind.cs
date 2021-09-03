using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
            if (GetComponent<UnityEngine.Canvas>()) return "Canvas";
            if (GetComponent<UnityEngine.CanvasGroup>()) return "CanvasGroup";
            if (GetComponent<UnityEngine.UI.RawImage>()) return "RawImage";
            if (GetComponent<UnityEngine.UI.Button>()) return "Button";
            if (GetComponent<UnityEngine.UI.Image>()) return "Image";
            if (GetComponent<UnityEngine.UI.Text>()) return "Text";
            if (GetComponent<UnityEngine.UI.ToggleGroup>()) return "ToggleGroup";
            if (GetComponent<UnityEngine.UI.Toggle>()) return "Toggle";
            if (GetComponent<UnityEngine.UI.Slider>()) return "Slider";
            if (GetComponent<UnityEngine.UI.Scrollbar>()) return "Scrollbar";
            if (GetComponent<UnityEngine.UI.Dropdown>()) return "Dropdown";
            if (GetComponent<UnityEngine.UI.ScrollRect>()) return "ScrollRect";
            if (GetComponent<UnityEngine.RectTransform>()) return "RectTransform";
            if (GetComponent<UnityEngine.Transform>()) return "Transform";
            if (GetComponent<UnityEngine.CanvasRenderer>()) return "CanvasRenderer";
            if (GetComponent<UnityEngine.UI.InputField>()) return "InputField";
         
            return string.Empty;
        }
    }


   
}
