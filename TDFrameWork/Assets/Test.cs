using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    public GameObject m_Obj;
  
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            //同步加载一个GameObject
            var op = Addressables.LoadAssetAsync<GameObject>("XXXView");
            GameObject go = op.WaitForCompletion();
            m_Obj= GameObject.Instantiate(go);

            //Do work...（运行……）

            Addressables.Release(op);

        }

    }


 
}


