﻿using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;

public class Test : MonoBehaviour
{
   
    private void Start()
    {
        GameEntry.Instance.OnSingletonInit();
    }
}


