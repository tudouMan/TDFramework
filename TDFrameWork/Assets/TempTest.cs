using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTest : MonoBehaviour
{

    private void Awake()
    {
        var instance = TDFramework.Net.NetTcp.Instance;
    }
    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "send"))
        {
            TDFramework.Net.NetTcp.Instance.Send(null);
        }
    }
}
