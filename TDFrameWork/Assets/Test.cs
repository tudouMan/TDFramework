using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    public GameObject m_Obj;
    public Shader m_Shader;
    private void Awake()
    {
        GameEntry.Instance.OnSingletonInit();
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            GameEntry.Res.InstanceAsync("Cube");
        }

    }


 
}


