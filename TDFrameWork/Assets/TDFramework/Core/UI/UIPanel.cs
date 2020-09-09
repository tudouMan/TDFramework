using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.Resource;
using TDFramework.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDFramework.UI
{

    public interface IUIData { }


    public abstract class UIPanel:MonoBehaviour,IPanel
    {

        protected UIPanelData mData;
        public UIPanelData PanelData { get => mData; set => mData = value; }

        public class UIPanelData : IUIData
        {

        }


        protected UIPanelInfo mUIPanelInfo;

        public UIPanelInfo PanelInfo { get => mUIPanelInfo; set => mUIPanelInfo = value; }

        public virtual void OnInit(IUIData uidata=null) { }

        public virtual void OnOpen(IUIData uidata = null) 
        {
            
        }

        public virtual void OnShow() { }

        public virtual void OnHide() { }

        public virtual void OnClose()
        {
            Res.ReleaseInstance(this.gameObject);
            GameObject.Destroy(this.gameObject);
            mUIPanelInfo = null;
        }




        /// <summary>
        /// event register on  awake
        /// </summary>
        public virtual void OnEventListenerRegister() { }

        public virtual void CloseSelf()
        {
            UIManager.Instance.ClosePanel(mUIPanelInfo.mPanelName);
        }

        public void Back()
        {
            UIManager.Instance.BackPanel(mUIPanelInfo.mPanelName);
        }

        /// <summary>
        /// event remove register on  destory
        /// </summary>
        public virtual void OnEventListenerRemoveRegister() { }

        private  void Awake()
        {
            OnEventListenerRegister();
        }

        private void OnDestroy()
        {
            OnEventListenerRemoveRegister();
        }

        public virtual void ClearData() { }
    }

}


public interface IPanel
{
    UIPanelInfo PanelInfo { get; set; }

    void OnInit(IUIData uidata = null);

    void OnOpen(IUIData uidata = null);

    void OnShow();

    void OnHide();

    void OnClose();

    void OnEventListenerRegister();

    void OnEventListenerRemoveRegister();
}

