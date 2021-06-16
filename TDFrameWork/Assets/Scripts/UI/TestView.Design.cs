using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public partial class TestView : UIPanel
    {
        public UnityEngine.UI.Image Image1;
        public UnityEngine.UI.Text Text1;
        public TMPro.TextMeshProUGUI Tmp1;

        public override void ClearData()
        {
            base.ClearData();
            Image1 = null;
            Text1 = null;
            Tmp1 = null;
        }

    }
}