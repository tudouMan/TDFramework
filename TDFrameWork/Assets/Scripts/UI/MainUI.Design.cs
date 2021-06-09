using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public partial class MainUI : UIPanel
    {
        public UnityEngine.UI.Text Text;
        public UnityEngine.UI.Image Image;

        public override void ClearData()
        {
            base.ClearData();
            Text = null;
            Image = null;
        }

    }
}