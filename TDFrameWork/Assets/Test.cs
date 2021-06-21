using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            TDFramework.GameEntry.Instance.OnSingletonInit();
            TDFramework.GameEntry.Debug.Log("#Player# Instantiated");
        }
    }
}
