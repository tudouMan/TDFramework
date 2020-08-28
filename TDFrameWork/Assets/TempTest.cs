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

public class TempTest : MonoBehaviour
{
    private void OnGUI()
    {
       if(GUI.Button(new Rect(100, 100, 100, 100), "open"))
        {
            UIManager.Instance.OpenPanel<TempView>(UILevel.Bg);
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "close"))
        {
            UIManager.Instance.ClosePanel<TempView>();
        }
    }

}


