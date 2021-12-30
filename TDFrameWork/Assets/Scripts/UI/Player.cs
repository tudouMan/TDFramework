using System;
using TDFramework.UI;
using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{

    public class PlayerData : UIPanel.UIPanelData
    {
    }
    public partial class Player : UIPanel
    {
        private PlayerData mData;
        
        public override void OnOpen(IUIData uidata = null)
        {
            base.OnOpen(uidata);
            mData = uidata as PlayerData ?? new PlayerData();
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