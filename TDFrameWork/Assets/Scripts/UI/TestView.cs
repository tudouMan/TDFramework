using System;
using TDFramework.UI;
using UnityEngine.UI;

namespace Game.UI
{

    public class TestViewData : UIPanel.UIPanelData
    {
    }
    public partial class TestView : UIPanel
    {
        public override void OnInit(IUIData uidata = null)
        {
            mData = uidata as TestViewData ?? new TestViewData();
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