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
            GameEntry.IL.RuntimeFunc("Assets/HotFix/testdll_res.bytes", "Assets/HotFix/testpdb_res.bytes", null);
        }
    }



    [ContextMenu("Transpose")]
    private void TransposeBytes()
    {
           
       
      
     
        //1.使用File.ReadAllBytes加载数据流并用一个变量临时暂存下
        byte[]dllBytes= File.ReadAllBytes(@"E:\TDFramework\TDFrameWork\Library\ScriptAssemblies\HotDefine.dll");
        byte[] psbBytes = File.ReadAllBytes(@"E:\TDFramework\TDFrameWork\Library\ScriptAssemblies\HotDefine.pdb");
        //2.新建一个文件以".bytes”结尾
        File.WriteAllBytes(@"E:\TDFramework\TDFrameWork\Assets\HotFix\testdll_res.bytes", dllBytes);
        File.WriteAllBytes(@"E:\TDFramework\TDFrameWork\Assets\HotFix\testpdb_res.bytes", psbBytes);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

    }



}


 



