using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public class KnapsackViewData : UIPanel.UIPanelData
    {
    }
    public partial class KnapsackView : UIPanel
    {
        public override void OnInit(IUIData uidata = null)
        {
            mData = uidata as KnapsackViewData ?? new KnapsackViewData();
        }

        public override void OnOpen(IUIData uidata = null)
        {
            base.OnOpen(uidata);
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