using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.Data;
using LitJson;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.UI;
using UnityEngine.U2D;
using TDFramework.Resource;
using TDFramework.HeapPool;
using TDFramework.UI;
using TDFramework.Audio;
using TDFramework.Extention;

public class TempTest : MonoBehaviour
{
    public Image mObj;
    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "play"))
        {
            UIMgr.OpenPanel<TempView>(UILevel.Bg);
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "music"))
        {
            UIMgr.OpenPanel<BackView>(UILevel.Pop);
        }

        if (GUI.Button(new Rect(100, 300, 100, 100), "music"))
        {
            UIMgr.PushPanel<TempView>();
        }
        if (GUI.Button(new Rect(100, 400, 100, 100), "music"))
        {
            UIMgr.PushPanel<BackView>();
        }

        if (GUI.Button(new Rect(100, 500, 100, 100), "music"))
        {
            UIMgr.BackPanel();
        }
    }

}


