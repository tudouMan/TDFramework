using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.UI;
using UnityEngine.UI;

public class TempView : UIPanel
{
    private Text mText;

    public override void OnOpen(IUIData uidata = null)
    {
        if (mText == null)
            mText = GetComponentInChildren<Text>();
        mText.text = "测试UI框架";
        base.OnOpen(uidata);
    }
}
