using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TDFramework.Extention;

public class FrameworkEditor :EditorWindow
{
    [MenuItem("TD/框架配置设定",false,-10)]
    public static void OpenEditor()
    {
        EditorWindow.GetWindow<FrameworkEditor>().Show();
    }

    bool isFoldoutUI;
    string mUIInitPath = "Scripts/UI/";
    private void OnGUI()
    {
        EditorGUILayout.LabelField("TDFramework", GUILayout.Width(200));

        isFoldoutUI = EditorGUILayout.Foldout(isFoldoutUI, "UI配置");
        if (isFoldoutUI)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            EditorGUILayout.LabelField($"当前UI脚本生成路径:", GUILayout.Width(100));
            mUIInitPath= EditorGUILayout.TextField(mUIInitPath, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("保存", GUILayout.Width(200), GUILayout.Height(30)))
            {
               bool hasChinese=mUIInitPath.HasChinese();
                if (hasChinese)
                    this.ShowNotification(new GUIContent("路径带有中文请检查"));

                bool hasSpace = mUIInitPath.HasSpace();
                if (hasSpace)
                    this.ShowNotification(new GUIContent("路径带有空格请检查"));


            }
        }

    }
}
