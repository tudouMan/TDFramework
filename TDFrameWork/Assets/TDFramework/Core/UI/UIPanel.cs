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
using TDFramework.Extention;
namespace TDFramework.UI
{

    public interface IUIData { }


    public abstract class UIPanel:MonoBehaviour,IPanel
    {
        public class UIPanelData : IUIData
        {

        }

        protected UIPanelInfo mUIPanelInfo;

        public UIPanelInfo PanelInfo { get => mUIPanelInfo; set => mUIPanelInfo = value; }

        public virtual void OnInit(IUIData uidata=null) 
        {
            mUIPanelInfo.m_State = PanelState.Init;
        }

        public virtual void OnOpen(IUIData uidata = null) 
        {
            mUIPanelInfo.m_State = PanelState.Open;
            OnShow();
        }

        public virtual void OnShow()
        {
            mUIPanelInfo.m_State = PanelState.Show;
        }

        public virtual void OnHide() 
        {
            mUIPanelInfo.m_State = PanelState.Hide;
        }

        public virtual void OnClose()
        {
            mUIPanelInfo.m_State = PanelState.Close;
            GameEntry.Res.ReleaseInstance(this.gameObject);
            GameObject.Destroy(this.gameObject);
            mUIPanelInfo = null;
        }




        /// <summary>
        /// event register on  awake
        /// </summary>
        public virtual void OnEventListenerRegister() { }

        public virtual void CloseSelf()
        {
            UIManager.Instance.ClosePanel(mUIPanelInfo.m_PanelName);
        }

        public void Back()
        {
            UIManager.Instance.BackPanel(mUIPanelInfo.m_PanelName);
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

