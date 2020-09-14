using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TDFramework;
using System.Text;
using System.Net;
using TDFramework.EventSystem;
using TDFramework.StateMachine;
using TDFramework.Extention;


public class TempTest : MonoBehaviour
{

    StateFactory sf = new StateFactory();

    IState curState;
    private void OnGUI()
    {
      
        if (GUI.Button(new Rect(100, 200, 100, 100), "set"))
        {
           
            sf.RegisterClassAttribute<GameStateAttribute>(AssemblyUtil.DefaultCSharpAssembly);
            curState= sf.ToState("IdleState");
        }
        if (GUI.Button(new Rect(100, 300, 100, 100), "set"))
        {


            curState= sf.ToState("RunState");
        }
    }


    private void Update()
    {
        if (curState != null)
            curState.OnUpdate(Time.deltaTime);
    }

 

  
   
}


