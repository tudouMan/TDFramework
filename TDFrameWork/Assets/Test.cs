using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{

    public Dictionary<int, UnityEngine.GameObject> Dic = new Dictionary<int, GameObject>();

 

    private void Awake()
    {
        GameEntry.Instance.Init();
    }

    [Button("Set")]
    private void Set()
    {

    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            int outside = 0;
            MethdInvoke[] methds = new MethdInvoke[2];
            for (int index = 0; index < 2; index++)
            {
                int inside=0;
                methds[index] = delegate 
                {
                    Debug.Log($"outside:{outside.ToString()} inside:{inside.ToString()}");
                    inside++;
                    outside++;
                };
            }


            methds[0]();
            methds[0]();
            methds[0]();
            methds[1]();
            methds[1]();
        }
    }


    public delegate void MethdInvoke();


}


 



