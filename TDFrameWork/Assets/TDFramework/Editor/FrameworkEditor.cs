using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TDFramework.Extention;
using System.Text;
using TDFramework.UI;
using System.Linq;
using System.Reflection;
using UniRx;
using System.Threading.Tasks;
using TDFramework;
using System.IO;
using UnityEditor.AddressableAssets.Settings;

public class FrameworkEditor :EditorWindow
{
    [MenuItem("TDFramework/框架配置设定",false,-10)]
    public static void OpenEditor()
    {
        EditorWindow.GetWindow<FrameworkEditor>().Show();
    }

    private bool m_IsFoldoutUI;
    private bool m_IsFoldOutExcel;
    private bool m_IsFoldOutIL;

    private static string m_UIInitPath = "/Scripts/UI/";
    private static string m_UINameSpacePath = "Game.UI";
    private FrameWorkPathConfig m_PathConfig;


    #region Excel Parame
    private Vector2 m_EncrptVector2;
    private string m_EncrptDataStr;
    #endregion


    private void OnEnable()
    {
        m_PathConfig = Resources.Load<FrameWorkPathConfig>("PathConfig");
        m_UIInitPath = m_PathConfig.m_UIScriptSavaPath;
        m_UINameSpacePath = m_PathConfig.m_UINameSpacePath;
    }



    private void OnGUI()
    {
        EditorGUILayout.LabelField("TDFramework", GUILayout.Width(200));

        m_IsFoldoutUI = EditorGUILayout.Foldout(m_IsFoldoutUI, "UI配置");
        if (m_IsFoldoutUI)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"当前UI脚本生成路径:", GUILayout.Width(100));
            m_UIInitPath = EditorGUILayout.TextField(m_UIInitPath, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"当前UI脚本命名空间:", GUILayout.Width(100));
            m_UINameSpacePath = EditorGUILayout.TextField(m_UINameSpacePath, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存", GUILayout.Width(200), GUILayout.Height(30)))
            {
                if (m_UIInitPath.HasChinese())
                {
                    this.ShowNotification(new GUIContent("路径带有中文请检查"));
                    return;
                }

                if (m_UIInitPath.HasSpace())
                {
                    this.ShowNotification(new GUIContent("路径带有空格请检查"));
                    return;
                }

                if (m_UINameSpacePath.HasChinese())
                {
                    this.ShowNotification(new GUIContent("命名空间带中文"));
                    return;
                }

                if (m_UINameSpacePath.HasSpace())
                {
                    this.ShowNotification(new GUIContent("命名空间带空格"));
                    return;
                }
                m_PathConfig.m_UIScriptSavaPath = m_UIInitPath;
                m_PathConfig.m_UINameSpacePath = m_UINameSpacePath;

            }
        }


        m_IsFoldOutExcel = EditorGUILayout.Foldout(m_IsFoldOutExcel, "Excel配置");
        if (m_IsFoldOutExcel)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"Excel地址:", GUILayout.Width(100));
            EditorGUILayout.LabelField(m_PathConfig.m_ExcelDataPath, GUILayout.Width(300));
            if (GUILayout.Button("选择Excel文本文件夹", GUILayout.Width(150), GUILayout.Height(20)))
            {
                string exportPath = EditorUtility.OpenFolderPanel("select path", "", "");
                if (!exportPath.IsNull())
                {
                    m_PathConfig.m_ExcelDataPath = exportPath;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"解析后数据保存地址:", GUILayout.Width(100));
            EditorGUILayout.LabelField(m_PathConfig.m_ExcelDataSavaPath, GUILayout.Width(300));
            if (GUILayout.Button("选择解析后数据文件夹", GUILayout.Width(150), GUILayout.Height(20)))
            {
                string exportPath = EditorUtility.OpenFolderPanel("select path", "", "");
                if (!exportPath.IsNull())
                {
                    m_PathConfig.m_ExcelDataSavaPath = exportPath;
                }
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"脚本生成保存地址:", GUILayout.Width(100));
            EditorGUILayout.LabelField(m_PathConfig.m_CShapScriptsSavaPath, GUILayout.Width(300));
            if (GUILayout.Button("选择脚本保存文件夹", GUILayout.Width(150), GUILayout.Height(20)))
            {
                string exportPath = EditorUtility.OpenFolderPanel("select path", "", "");
                if (!exportPath.IsNull())
                {
                    m_PathConfig.m_CShapScriptsSavaPath = exportPath;
                }
            }
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("解析整文件夹", GUILayout.Width(200), GUILayout.Height(30)))
            {
                List<string> paths = m_PathConfig.m_ExcelDataPath.GetDirSubFilePathList(suffix: "xlsx");
                if (paths != null)
                {
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].IsNull())
                        {
                            ExportData(paths[i], m_PathConfig.m_ExcelDataSavaPath);
                        }
                    }

                }

            }


            EditorGUILayout.LabelField($"单个解析只需要点击按钮选择对应文件即可:", GUILayout.Width(300));
            if (GUILayout.Button("单个解析", GUILayout.Width(200), GUILayout.Height(30)))
            {
                string exportPath = EditorUtility.OpenFilePanel("select path", "", "xlsx");

                if (!exportPath.IsNull())
                {
                    ExportData(exportPath, m_PathConfig.m_ExcelDataSavaPath);
                }

            }



            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField($"解密文件:", GUILayout.Width(300));
            m_EncrptVector2 = EditorGUILayout.BeginScrollView(m_EncrptVector2, GUILayout.Width(500), GUILayout.Height(100));
            m_EncrptDataStr = EditorGUILayout.TextArea(m_EncrptDataStr, GUILayout.Width(400), GUILayout.Height(400));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("解密查看文件", GUILayout.Width(200), GUILayout.Height(30)))
            {
                string exportPath = EditorUtility.OpenFilePanel("select path", "", "json");
                string dataStr = System.IO.File.ReadAllText(exportPath);
                dataStr = TDFramework.Tool.StringEncryption.DecryptDES(dataStr);
                m_EncrptDataStr = dataStr;
            }

        }


        m_IsFoldOutIL = EditorGUILayout.Foldout(m_IsFoldOutIL, "IL配置");
        if (m_IsFoldOutIL)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
            EditorGUILayout.LabelField($"AssemblyName:", GUILayout.Width(100), GUILayout.Height(20));
            m_PathConfig.m_ILAssemblyDefinitionName = EditorGUILayout.TextField(m_PathConfig.m_ILAssemblyDefinitionName, GUILayout.Width(200), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Height(300));
            if (GUILayout.Button("转换DLLPDB:", GUILayout.Width(150), GUILayout.Height(20)))
            {
                
                DirectoryInfo pathInfo = new DirectoryInfo(Application.dataPath);
                string dllPath = pathInfo.Parent.FullName+ @"\Library\ScriptAssemblies\" + m_PathConfig.m_ILAssemblyDefinitionName + ".dll";
                if (!File.Exists(dllPath))
                {
                    Debug.LogError($"该路径下{pathInfo.Parent.FullName + @"\Library\ScriptAssemblies\"}没有{ m_PathConfig.m_ILAssemblyDefinitionName}.dll 请检查");
                    return;
                }
                string pdbPath = pathInfo.Parent.FullName + @"\Library\ScriptAssemblies\" + m_PathConfig.m_ILAssemblyDefinitionName + ".pdb";
                if (!File.Exists(pdbPath))
                {
                    Debug.LogError($"该路径下{pathInfo.Parent.FullName + @"\Library\ScriptAssemblies\"}没有{ m_PathConfig.m_ILAssemblyDefinitionName}.pdb 请检查");
                    return;
                }


               

                byte[]dllBytes= File.ReadAllBytes(dllPath);
                byte[] pdbBytes = File.ReadAllBytes(pdbPath);
                //2.新建一个文件以".bytes”结尾
                string toDllPath = Application.dataPath + @"\Res\HotFix\dll_res.bytes";
                string toPdbPath = Application.dataPath + @"\Res\HotFix\pdb_res.bytes";

                
                if (File.Exists(toDllPath))
                    File.Delete(toDllPath);
               

                if (File.Exists(toPdbPath))
                    File.Delete(toPdbPath);
               
               

                File.WriteAllBytes(toDllPath, dllBytes);
                Debug.Log("----------DLL转换成功 ToPath:"+ toDllPath);
                File.WriteAllBytes(toPdbPath, pdbBytes);
                Debug.Log("----------PDB转换成功 ToPath:"+ toPdbPath);
                UnityEditor.AssetDatabase.Refresh();
            }
            EditorGUILayout.EndVertical();
        }

        UnityEditor.EditorUtility.SetDirty(m_PathConfig);
    }




public static void ExportData(string exportPath,string savaPath)
    {
        //-- Load Excel
        ExcelLoader excel = new ExcelLoader(exportPath, 3, exportPath.GetFileNameWithoutExtend());
        //-- export
        JsonExporter exporter = new JsonExporter(excel);
        exporter.SaveToFile(savaPath + "/" + exportPath.GetFileNameWithoutExtend() + ".json", Encoding.UTF8);


        CSharpExporter csharp = new CSharpExporter(excel.Sheets[0], exportPath.GetFileNameWithoutExtend());
        csharp.CreatCSharpScript();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
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
        
        string savaPath = m_UIInitPath;
        savaPath = string.Format(Application.dataPath + m_UIInitPath);
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
            contentBuilder.Append("using UnityEngine;");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceLine);
            contentBuilder.Append($"namespace {m_UINameSpacePath}");
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

            // private TestViewData mData;
            contentBuilder.Append($"private {name}Data mData;");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);

            contentBuilder.Append("public override void OnInit(IUIData uidata = null)");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceNine);
            contentBuilder.Append("{");
            contentBuilder.Append(spaceLine);
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append("base.OnInit(uidata);");
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
            contentBuilder.Append(spaceTwelve);
            contentBuilder.Append($"mData = uidata as {uiDataName} ?? new {uiDataName}();");
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
        
        bindBuilder.Append("using UnityEngine;");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append("using UnityEngine.UI;");
        bindBuilder.Append(spaceLine);
        bindBuilder.Append($"namespace {m_UINameSpacePath}");
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



    //  [UnityEditor.Callbacks.DidReloadScripts]
    [InitializeOnLoadMethod]
    static void UICreatRun()
    {
        if (EditorPrefs.GetBool("UICreatRun"))
        {
            GameObject SelectObj = Selection.activeGameObject;
            if (SelectObj == null)
            {
                return;
            }
            string SelectName = SelectObj.name;
            var Binds = SelectObj.GetComponentsInChildren<UIBind>();
            if (!SelectObj.GetComponent(SelectName))
            {
                var assembly = TDFramework.Extention.ReflectionExtension.GetAssemblyCSharp();
                string nameSpace = Resources.Load<FrameWorkPathConfig>("PathConfig").m_UINameSpacePath;
                System.Type addType = assembly.GetType(nameSpace +"."+ SelectName);
                SelectObj.AddComponent(addType);
            }
            var viewType = SelectObj.GetComponent(SelectName);
            System.Type type = viewType.GetType();


            foreach (var item in Binds)
            {
              
               
                string propertityName = item.gameObject.name;
                //获取到分裂类中对应需要设置的属性
                var fileInfo = type.GetField(propertityName);
                if (fileInfo != null)
                {
                    Component itemComponent = item.transform.GetComponent(item.ComponentName);
                  
                    if (itemComponent == null)
                    {
                        Debug.LogWarning($"Behavior{item.gameObject.name}物体上没有{item.DefaultName()}属性");
                    }

                    //设置属性对应的component
                    fileInfo.SetValue(viewType, itemComponent);
                    EditorUtility.SetDirty(SelectObj);
                }
                else
                    Debug.LogWarning($"字段没有{item.gameObject.name}物体上没有{item.DefaultName()}属性");


            }
        
           
            Debug.Log($"{SelectName}Script creat Success..............");
            EditorPrefs.SetBool("UICreatRun", false);
           
        }
       
    }

    

}




[CustomEditor(typeof(UIBind), true)]
public class UIBindInspector : Editor
{

    string[] bindTypeStrs = new string[] { "Default", "Custom" };
  

    BindType curBindType;
    string curSelectType;


    string[] selectTypes = new string[]
    {
        "TMPro.TextMeshProUGUI",
        "TMPro.TMP_InputField",
        "TMPro.TMP_Dropdown",
        "TMPro.TMP_InputField",
        "Canvas",
        "CanvasGroup",
        "RawImage",
        "Button",
        "Image",
        "Text",
        "ToggleGroup",
        "Toggle",
        "Slider",
        "Scrollbar",
        "Dropdown",
        "ScrollRect",
        "RectTransform",
        "Transform",
        "CanvasRenderer",
        "InputField",
    };

    int customTypeIndex = 0;
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
            Undo.RegisterCompleteObjectUndo(bind, "modify value");
            EditorUtility.SetDirty(bind);
            AssetDatabase.SaveAssets();

        }

    }


  
}


