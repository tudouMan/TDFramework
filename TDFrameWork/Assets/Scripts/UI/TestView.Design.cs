using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public partial class TestView : UIPanel
    {
        public UnityEngine.UI.Button Button;

        public override void ClearData()
        {
            base.ClearData();
            Button = null;
        }

    }
}