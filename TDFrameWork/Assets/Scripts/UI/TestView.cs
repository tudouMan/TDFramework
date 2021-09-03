using System;
using TDFramework.UI;
using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{

    public class TestViewData : UIPanel.UIPanelData
    {
    }
    public partial class TestView : UIPanel
    {
        private TestViewData mData;
        public override void OnInit(IUIData uidata = null)
        {
            base.OnInit(uidata);
        }

        public override void OnOpen(IUIData uidata = null)
        {
            base.OnOpen(uidata);
            mData = uidata as TestViewData ?? new TestViewData();
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