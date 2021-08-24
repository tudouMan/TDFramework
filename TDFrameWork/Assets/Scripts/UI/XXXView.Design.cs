using System;
using TDFramework.UI;

namespace Game.UI
{

    public partial class XXXView : UIPanel
    {
        public UnityEngine.Transform GameObject;
        public UnityEngine.UI.Image Image;

        public override void ClearData()
        {
            base.ClearData();
            GameObject = null;
            Image = null;
        }

    }
}