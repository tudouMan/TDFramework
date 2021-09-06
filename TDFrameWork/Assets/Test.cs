using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;



public class Test : MonoBehaviour
{
    private void Awake()
    {
        GameEntry.Instance.Init();
    }


    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            Debug.Log("set:" + GameEntry.Table.m_TestTable.Get(1).set.ToString());
        }
    }
}


 



