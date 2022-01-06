using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix;
namespace TDFramework
{
    public class PoolInspectorView : EditorWindow
    {
        [MenuItem("TDFramework/对象池查看器", false)]
        public static void Open()
        {
            GetWindow<PoolInspectorView>().Show();
        }

        private bool m_PrefabFold;
        private bool m_ClassFold;
        private Vector2 m_PrefabPos;
        private Vector2 m_ClassPos;

      

        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                Dictionary<string, int> dic = GameEntry.Pool.PrefabPool.PoolInspectorDic;
                m_PrefabFold = EditorGUILayout.Foldout(m_PrefabFold, "预制体查看");
                if (m_PrefabFold)
                {
                    m_PrefabPos = EditorGUILayout.BeginScrollView(m_PrefabPos, GUILayout.Width(150), GUILayout.Height(300));
                    foreach (var item in dic)
                    {
                        EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
                        EditorGUILayout.LabelField("PoolName:" + item.Key);
                        EditorGUILayout.LabelField("PoolCount:" + item.Value);
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();

                }
            }
        }
    }
}
