using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public class XXXViewData : UIPanel.UIPanelData
    {
    }
    public partial class XXXView : UIPanel
    {
        private XXXViewData mData;
        public override void OnInit(IUIData uidata = null)
        {
            base.OnInit(uidata);
        }

        public override void OnOpen(IUIData uidata = null)
        {
            base.OnOpen(uidata);
            mData = uidata as XXXViewData ?? new XXXViewData();
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnClose()
        {
             base.OnClose();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnEventListenerRegister()
        {
            base.OnEventListenerRegister();
        }

        public override void OnEventListenerRemoveRegister()
        {
            base.OnEventListenerRemoveRegister();
        }

    }
}