using TDFramework.UI;
using UnityEngine;

namespace TDFramework.UI
{

    public interface IUIData { }


    public abstract class UIPanel:MonoBehaviour,IPanel
    {
        public class UIPanelData : IUIData
        {

        }

        protected UIPanelInfo m_UIPanelInfo;

        public UIPanelInfo PanelInfo { get => m_UIPanelInfo; set => m_UIPanelInfo = value; }


        public virtual void OnOpen(IUIData uidata = null) 
        {
            m_UIPanelInfo.m_State = PanelState.Open;
            OnShow();
        }

        public virtual void OnShow()
        {
            m_UIPanelInfo.m_State = PanelState.Show;
        }

        public virtual void OnHide() 
        {
            m_UIPanelInfo.m_State = PanelState.Hide;
        }

        public virtual void OnClose()
        {
            m_UIPanelInfo.m_State = PanelState.Close;
            GameEntry.Res.ReleaseInstance(this.gameObject);
            GameObject.Destroy(this.gameObject);
            m_UIPanelInfo = null;
        }


        /// <summary>
        /// event register on  awake
        /// </summary>
        public virtual void OnEventListenerRegister() { }

        public virtual void CloseSelf()
        {
            UIManager.Instance.ClosePanel(m_UIPanelInfo.m_PanelName);
        }

        public void Back()
        {
            UIManager.Instance.BackPanel(m_UIPanelInfo.m_PanelName);
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

    void OnOpen(IUIData uidata = null);

    void OnShow();

    void OnHide();

    void OnClose();

    void OnEventListenerRegister();

    void OnEventListenerRemoveRegister();
}

