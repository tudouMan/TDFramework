
using UnityEngine;

namespace TDFramework.UI
{
    public partial class TempView : UIPanel
    {
        public UnityEngine.UI.Text BtnSelf;


        public override void ClearData()
        {
            base.ClearData();
            BtnSelf = null;
        }


    }
}
