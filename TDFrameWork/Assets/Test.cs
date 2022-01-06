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
            GameEntry.Time.CreatTimeAction().Init("testTimeAction", 0.1f, 0.3f,5,startAction:()=> { GameEntry.Debug.Log("start"); },
                runAction: p => { GameEntry.Debug.Log("runtime:" + p.ToString()); },
                completeAction: () => { GameEntry.Debug.Log("setTime"); }).Run();
        }

       
    }

   
}


 



