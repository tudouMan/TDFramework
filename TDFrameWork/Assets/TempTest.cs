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

public class TempTest : MonoBehaviour
{
    private void OnGUI()
    {
       if(GUI.Button(new Rect(100, 100, 100, 100), "play"))
        {
            SoundManager.Instance.PlaySound("monsterBorn", 1);
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "music"))
        {
            SoundManager.Instance.PlayMusic("backroom", 1);
        }

        if (GUI.Button(new Rect(100, 300, 100, 100), "music"))
        {
            SoundManager.Instance.PlayMusic("LoadSpace", 1);
        }
    }

}


