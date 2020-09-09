using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TDFramework.Extention;
using System.Text;
using TDFramework.UI;
using System.Linq;
using System.Reflection;
using UniRx;
using System.Threading.Tasks;

public class FrameworkEditor :EditorWindow
{
    [MenuItem("TDFramework/框架配置设定",false,-10)]
    public static void OpenEditor()
    {
        EditorWindow.GetWindow<FrameworkEditor>().Show();
    }

    bool mIsFoldoutUI;
    static string mUIInitPath = "/Scripts/UI/";
    static string mUINameSpacePath = "Game.UI";

    static string mSaveUIInitPath = "UIInitPath";
    static string mSaveUINameSpacePath = "UINameSpacePath";



    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(mSaveUIInitPath))
            mUIInitPath = PlayerPrefs.GetString(mSaveUIInitPath);

        if (PlayerPrefs.HasKey(mSaveUINameSpacePath))
            mUINameSpacePath = PlayerPrefs.GetString(mSaveUINameSpacePath);
    }


    private void OnGUI()
    {
        EditorGUILayout.LabelField("TDFramework", GUILayout.Width(200));

        mIsFoldoutUI = EditorGUILayout.Foldout(mIsFoldoutUI, "UI配置");
        if (mIsFoldoutUI)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"当前UI脚本生成路径:", GUILayout.Width(100));
            mUIInitPath= EditorGUILayout.TextField(mUIInitPath, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"当前UI脚本命名空间:", GUILayout.Width(100));
            mUINameSpacePath = EditorGUILayout.TextField(mUINameSpacePath, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存", GUILayout.Width(200), GUILayout.Height(30)))
            {
                if (mUIInitPath.HasChinese())
                {
                    this.ShowNotification(new GUIContent("路径带有中文请检查"));
                    return;
                }

                if (mUIInitPath.HasSpace())
                {
                    this.ShowNotification(new GUIContent("路径带有空格请检查"));
                    return;
                }

                if (mUINameSpacePath.HasChinese())
                {
                    this.ShowNotification(new GUIContent("命名空间带中文"));
                    return;
                }

                if (mUINameSpacePath.HasSpace())
                {
                    this.ShowNotification(new GUIContent("命名空间带空格"));
                    return;
                }

                PlayerPrefs.SetString(mSaveUIInitPath, mUIInitPath);
                PlayerPrefs.SetString(mSaveUINameSpacePath, mUIInitPath);
            }
        }

    }


    [MenuItem("Assets/创建UI脚本/Creat",false,-10)]
    public static  void CreatUIScripts()
    {

        GameObject selectObj = Selection.activeGameObject;
        if (selectObj == null)
        {
            Debug.LogError("没有选中物体请选择");
            return;
        }
        
        string savaPath = mUIInitPath;
        if (PlayerPrefs.HasKey(mSaveUIInitPath))
            savaPath = PlayerPrefs.GetString(mSaveUIInitPath);
        savaPath = string.Format(Application.dataPath + mUIInitPath);
        savaPath.CreateDirIfNotExists();
        EditorPrefs.SetBool("UICreatRun", true);
        CreatUI(savaPath, selectObj.name, selectObj);
    }



 
    public static  void CreatUI(string path,string name,GameObject selectObj)
    {
        string spaceLine = "\n";
        string uiDataName = $"{name}Data";
        string spaceOne = " ";
        string spaceFour = "    ";
        string spaceNine = "        ";
        string spaceTwelve = "            ";
       
        if (!System.IO.File.Exists(path + name + ".cs"))
        {
            #region CS
            StringBuilder contentBuilder = new StringBuilder();
           

            //using title
            contentBuilder.Append("using System;");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append("using TDFramework.UI;");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append("using UnityEngine.UI;");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceLine);

            if (PlayerPrefs.HasKey(mSaveUINameSpacePath))
                mUINameSpacePath = PlayerPrefs.GetString(mSaveUINameSpacePath);

            contentBuilder.Append($"namespace {mUINameSpacePath}");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceLine);

            contentBuilder.Append($"{spaceFour}public{spaceOne}class{spaceOne}{uiDataName}{spaceOne}:{spaceOne}UIPanel.UIPanelData");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append($"{spaceFour}");
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append($"{spaceFour}");
            contentBuilder.Append("}");


            contentBuilder.Append(spaceLine);

            //content
            contentBuilder.Append($"{spaceFour}public partial{spaceOne}class{spaceOne}{name}{spaceOne}:{spaceOne}UIPanel");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceFour + "{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);


            contentBuilder.Append("public override void OnInit(IUIData uidata = null)");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append($"mData = uidata as {uiDataName} ?? new {uiDataName}();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);

            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnOpen(IUIData uidata = null)");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnOpen(uidata);");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnShow()");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnShow();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnClose()");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append(" base.OnClose();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnHide()");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnHide();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnEventListenerRegister()");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnEventListenerRegister();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("public override void OnEventListenerRemoveRegister()");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnEventListenerRemoveRegister();");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceLine);


            contentBuilder.Append(spaceFour);
            contentBuilder.Append("}");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append("}");

            System.IO.File.WriteAllText(path + name + ".cs", contentBuilder.ToString());
            AssetDatabase.Refresh();

            #endregion
        }




        #region DeSign
        var binds =selectObj.GetComponentsInChildren<UIBind>();
        StringBuilder bindBuilder = new StringBuilder();

        bindBuilder.Append("using System;");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append("using TDFramework.UI;");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append("using UnityEngine.UI;");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceLine);


        if (PlayerPrefs.HasKey(mSaveUINameSpacePath))
            mUINameSpacePath = PlayerPrefs.GetString(mSaveUINameSpacePath);

        bindBuilder.Append($"namespace {mUINameSpacePath}");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append("{");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceLine);


        bindBuilder.Append($"{spaceFour}public partial{spaceOne}class{spaceOne}{name}{spaceOne}:{spaceOne}UIPanel");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceFour + "{");


        //Content
        foreach (var item in binds)
        {
            bindBuilder.Append(spaceLine);
            bindBuilder.Append(spaceNine);
            bindBuilder.Append($"public { item.ComponentName} {item.gameObject.name};");
        }
     




        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceLine);

        //ClearData
        bindBuilder.Append(spaceNine);
        bindBuilder.Append("public override void ClearData()");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceNine);
        bindBuilder.Append("{");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceTwelve);
        bindBuilder.Append("base.ClearData();");


        //具体执行的内容
        foreach (var item in binds)
        {
            bindBuilder.Append(spaceLine);
            bindBuilder.Append(spaceTwelve);
            bindBuilder.Append($"{item.gameObject.name} = null;");
        }

        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceNine);
        bindBuilder.Append("}");

        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceLine);
        bindBuilder.Append(spaceFour);
        bindBuilder.Append("}");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append("}");


        System.IO.File.WriteAllText(path + name + ".Design.cs", bindBuilder.ToString());


        AssetDatabase.Refresh();
        #endregion
        
    }



    [InitializeOnLoadMethod]
    static void UICreatRun()
    {
        if (EditorPrefs.GetBool("UICreatRun"))
        {

            GameObject SelectObj = Selection.activeGameObject;
            if (SelectObj == null)
            {
                Debug.LogError("没有选中物体请选择");
                return;
            }
            string SelectName = SelectObj.name;
            var Binds = SelectObj.GetComponentsInChildren<UIBind>();
            if (!SelectObj.GetComponent(SelectName))
            {
                var assembly = TDFramework.Extention.ReflectionExtension.GetAssemblyCSharp();
                System.Type addType = assembly.GetType("TDFramework.UI." + SelectName);
                SelectObj.AddComponent(addType);
            }
            var component = SelectObj.GetComponent(SelectName);
            System.Type type = component.GetType();
            foreach (var item in Binds)
            {
                string propertityName = item.gameObject.name;
                var fileInfo = type.GetField(propertityName);
                if (fileInfo != null)
                {
                    if (item.gameObject.GetComponent(item.ComponentName) == null)
                    {
                        Debug.LogWarning($"Behavior{item.gameObject.name}物体上没有{item.DefaultName()}属性");
                    }
                    fileInfo.SetValue(component, item.gameObject.GetComponent(item.ComponentName));
                }
                else
                    Debug.LogWarning($"字段没有{item.gameObject.name}物体上没有{item.DefaultName()}属性");

            }

            Debug.Log($"{SelectName}Script Build Success..............");
            EditorPrefs.SetBool("UICreatRun", false);
        }

       
    }
}




[CustomEditor(typeof(UIBind), true)]
public class UIBindInspector : Editor
{

    string[] bindTypeStrs = new string[] { "Default", "Custom" };
    int customTypeIndex = 17;

    BindType curBindType;
    string curSelectType;


    string[] selectTypes = new string[]
    {
        "TMPro.TextMeshProUGUI",
        "TMPro.TMP_InputField",
        "TMPro.TMP_Dropdown",
        "TMPro.TMP_InputField",
        "UnityEngine.Canvas",
        "UnityEngine.CanvasGroup",
        "UnityEngine.UI.RawImage",
        "UnityEngine.UI.Button",
        "UnityEngine.UI.Image",
        "UnityEngine.UI.Text",
        "UnityEngine.UI.ToggleGroup",
        "UnityEngine.UI.Toggle",
        "UnityEngine.UI.Slider",
        "UnityEngine.UI.Scrollbar",
        "UnityEngine.UI.Dropdown",
        "UnityEngine.UI.ScrollRect",
        "UnityEngine.RectTransform",
        "UnityEngine.GameObject",
        "UnityEngine.Transform",
        "UnityEngine.CanvasRenderer"
    };


    public override void OnInspectorGUI()
    {
        bool isRefresh = false;
        UIBind bind = target as UIBind;
        EditorGUILayout.BeginVertical(GUILayout.Width(600), GUILayout.Height(100));
        EditorGUILayout.LabelField("选择对应的类型");


        curBindType = bind.BindType;
        bind.BindType = (BindType)EditorGUILayout.Popup(bind.BindType.GetHashCode(), bindTypeStrs, GUILayout.Width(100));
        if(curBindType!= bind.BindType)
            isRefresh = true;
       
        curBindType = bind.BindType;


        switch (bind.BindType)
        {
            case BindType.Default:
                EditorGUILayout.LabelField("默认类型为:" + bind.DefaultName());
                bind.SelectTypeName = string.Empty;
                break;
            case BindType.Custom:
                curSelectType = bind.SelectTypeName;
                EditorGUILayout.BeginHorizontal(GUILayout.Width(600), GUILayout.Height(50));
                EditorGUILayout.LabelField("选择预定义Type：", GUILayout.Width(100));
                if (string.IsNullOrEmpty(bind.SelectTypeName))
                    bind.SelectTypeName = selectTypes[customTypeIndex];
                customTypeIndex = selectTypes.ToList().IndexOf(bind.SelectTypeName);
                customTypeIndex = EditorGUILayout.Popup(customTypeIndex, selectTypes, GUILayout.Width(100));
                bind.SelectTypeName = selectTypes[customTypeIndex];
                if(curSelectType!= bind.SelectTypeName)
                    isRefresh = true;

                curSelectType = bind.SelectTypeName;
                EditorGUILayout.EndHorizontal();
                break;
            default:
                break;
        }

        EditorGUILayout.EndVertical();
        base.OnInspectorGUI();

        if (isRefresh)
        {
            Undo.RecordObject(bind, "modify value");
            EditorUtility.SetDirty(bind);
        }
    }


  
}



