using UnityEngine;
using System.Collections.Generic;
using TDFramework;
using System;
using UnityEngine.AddressableAssets;
using System.IO;
using System.Reflection;
using PureScript;

public class Test : MonoBehaviour
{
    private void Start()
    {
        GameEntry.Instance.Init();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            GameEntry.Runtime.RuntimeFunc("Game.HotManager", "Debug",new object[1] { 1});
        }
    }
}


 



