using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class GameTest : MonoBehaviour
{
    private GameObject m_GameObject;
    private void Awake()
    {
        GameEntry.Instance.OnSingletonInit();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "pop"))
        {
            GameEntry.Pool.PopPrefab("TestPrefab", p => m_GameObject = p.gameObject);
        }
        if (GUI.Button(new Rect(100, 200, 100, 100), "push"))
        {
            GameEntry.Pool.PushPrefab(m_GameObject);
        }

    }
}
