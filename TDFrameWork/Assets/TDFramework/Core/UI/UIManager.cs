using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.Resource;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace TDFramework.UI
{
    public enum UILevel
    {
        /// <summary>
        /// 最底层
        /// </summary>
        Botton,
        /// <summary>
        /// 背景层
        /// </summary>
        Bg,
        /// <summary>
        /// 常驻层
        /// </summary>
        Fixed,
        /// <summary>
        /// 弹窗层
        /// </summary>
        Pop,
        /// <summary>
        /// 最高层级物体或特效层
        /// </summary>
        Top,
        /// <summary>
        /// 设计层级
        /// </summary>
        Design,
    }

    [MonoSingletonPath("UIRoot")]
    public class UIManager : MonoBehaviour, ISingleton
    {

        #region propertity

        [SerializeField] private Camera mUICamera;
        [SerializeField] private Canvas mUICanvas;
        [SerializeField] private Transform[] mUILayers;
        private Stack<UIPanelInfo> mStacks;
        private Dictionary<string, IPanel> mLoadPanels;
        #endregion

        public Camera UICamera { get => mUICamera; }
        public Canvas UICanvas { get => mUICanvas; }

        private Transform GetTransformByUILevel(UILevel level)
        {
            return mUILayers[level.GetHashCode()];
        }

        public void OnSingletonInit()
        {
            mLoadPanels = new Dictionary<string, IPanel>();
            mStacks = new Stack<UIPanelInfo>();
            GetTransformByUILevel(UILevel.Design).gameObject.SetActive(false);
        }

        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UIManager>();
                    if (instance == null)
                    {
                       GameObject uiRoot=GameObject.Instantiate(Resources.Load<GameObject>("UIRoot"));
                        uiRoot.name = "UIRoot";
                        DontDestroyOnLoad(uiRoot);
                        instance = MonoSingletonProperty<UIManager>.Instance;
                    }
                }

                return instance;


            }
        }

        public IPanel GetPanel(string panelName)
        {
            IPanel panel = null;
            if (!mLoadPanels.TryGetValue(panelName, out panel))
                return null;
            else
                return panel;
                
        }

        public IPanel GetPanel<T>()where T : IPanel
        {
            string panelName=typeof(T).Name;
            return GetPanel(panelName);
        }

        public  void OpenPanel(string panelName, UILevel uilevel,IUIData uidata=null)
        {
            if (string.IsNullOrEmpty(panelName))
                throw new Exception("panelName is null");

            IPanel uipanel = null;
            if(!mLoadPanels.TryGetValue(panelName,out uipanel))
            {
              
               Res.InstanceAsync(panelName, panel =>
               {
                      if (!mLoadPanels.TryGetValue(panelName, out uipanel))
                      {
                       Transform trans = GetTransformByUILevel(uilevel);
                       panel.transform.SetParent(trans);
                       uipanel = panel.GetComponent<UIPanel>();
                       panel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                       panel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                       panel.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                       uipanel.PanelInfo = new UIPanelInfo()
                       {
                           mPanelLevel = uilevel,
                           mPanelData = uidata,
                           mPanelName = panelName,
                       };

                       uipanel.OnOpen(uidata);
                          uipanel.OnShow();
                          mLoadPanels.Add(panelName,uipanel);
                          
                      }
                  });
            }
            else
            {
                uipanel.OnOpen(uidata);
                uipanel.OnShow();
            }

        }

        public void OpenPanel<T>(UILevel uilevel, IUIData uidata = null)
        {
            OpenPanel(typeof(T).Name, uilevel, uidata);
        }

        public void ClosePanel(string panelName)
        {
            IPanel panel = null;
            if (mLoadPanels.TryGetValue(panelName,out panel))
            {
                panel.OnClose();
                mLoadPanels.Remove(panelName);
            }
        }

        public void ClosePanel<T>()
        {
            ClosePanel(typeof(T).Name);
        }

        public void ShowPanel(string panelName)
        {
            IPanel panel = GetPanel(panelName);
            if(panel != null)
                panel.OnShow();
        }

        public void ShowPanel<T>()
        {
            ShowPanel(typeof(T).Name);
        }

        public void HidePanel(string panelName)
        {
            IPanel panel = GetPanel(panelName);
            if (panel != null)
                panel.OnHide();
        }

        public void HidePanel<T>()
        {
            HidePanel(typeof(T).Name);
        }

        public void PushPanel(IPanel panel)
        {
            if (panel.PanelInfo != null)
            {
                mStacks.Push(panel.PanelInfo);
                panel.OnClose();
                mLoadPanels.Remove(panel.PanelInfo.mPanelName);
            }
               
        }

        public void PushPanel<T>()where T:IPanel
        {
           IPanel panel=GetPanel<T>();
            if (panel != null)
                PushPanel(panel);
        }

        public void BackPanel(string curPanelName)
        {
            if (!string.IsNullOrEmpty(curPanelName))
            {
                ClosePanel(curPanelName);
            }
            var uiPanelData = mStacks.Pop();
            OpenPanel(uiPanelData.mPanelName, uiPanelData.mPanelLevel, uiPanelData.mPanelData);
        }
    }

    
}
