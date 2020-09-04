using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.Data;
using LitJson;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using System.Resources;
using UnityEngine.UI;
using UnityEngine.U2D;
using TDFramework.Resource;
using TDFramework.HeapPool;
using TDFramework.UI;
using TDFramework.Audio;
using TDFramework.Extention;
using UnityEngine.ResourceManagement.ResourceLocations;
using UniRx;

public class TempTest : MonoBehaviour
{

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            UIMgr.OpenPanel<TempView>(UILevel.Botton);
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "set"))
        {
            UIMgr.OpenPanel<BackView>(UILevel.Pop);
            
        }
    }
}


