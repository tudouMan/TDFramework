using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDMsgSpan
{
    public static readonly int FrameworkID = 0;
}

partial class TDMsgID
{
  
    public static readonly int UI = TDMsgSpan.FrameworkID + 2000;
    public static readonly int Event = UI + 2000;
}