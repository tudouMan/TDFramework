using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.UI;
using UnityEngine.UI;

public class BackView : UIPanel
{
    public override void OnInit(IUIData uidata = null)
    {
        base.OnInit(uidata);
    }

    public override void OnOpen(IUIData uidata = null)
    {
        base.OnOpen(uidata);
        GetComponentInChildren<Text>().text = "Back";
    }


}
