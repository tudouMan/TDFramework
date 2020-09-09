using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public partial class KnapsackView : UIPanel
    {
        public UnityEngine.UI.Text Text;

        public override void ClearData()
        {
            base.ClearData();
            Text = null;
        }

    }
}