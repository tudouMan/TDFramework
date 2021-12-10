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


    public GameObject obj;
 

    private void Awake()
    {
        GameEntry.Instance.Init();
    }



    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
          //  GameEntry.IL.RuntimeFunc("Assets/HotFix/dll_res", "Assets/HotFix/pdb_res", null);
        }
    }



    [ContextMenu("Transpose")]
    private void TransposeBytes()
    {
           
        

    }



}


 



