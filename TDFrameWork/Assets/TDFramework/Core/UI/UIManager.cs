using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.Extention;
using TDFramework.Resource;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UniRx;
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
        private Stack<UIPanelInfo> mStacks=new Stack<UIPanelInfo>();
        private Dictionary<string, IPanel> mLoadPanels=new Dictionary<string, IPanel>();
        #endregion


        public Camera UICamera { get => mUICamera; }
        public Canvas UICanvas { get => mUICanvas; }

        private Transform GetTransformByUILevel(UILevel level)
        {
            return mUILayers[level.GetHashCode()];
        }

        public void OnSingletonInit()
        {
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

            IPanel loadpanel = null;
            if(!mLoadPanels.TryGetValue(panelName,out loadpanel))
            {
                CreatPanel(panelName, uilevel, uidata);
            }
           

            Observable.EveryUpdate().
                Where(_ => loadpanel != null )
                .Subscribe(_ => 
                {
                    loadpanel.OnOpen(uidata);
                    loadpanel.OnShow();

                }).AddTo(this);

        }

        private void CreatPanel(string panelName, UILevel uilevel, IUIData uidata = null)
        {
            Res.InstanceAsync(panelName, panel =>
            {
                IPanel outPanel = null;
                if (!mLoadPanels.TryGetValue(panelName, out outPanel))
                {
                   panel.Parent(GetTransformByUILevel(uilevel))
                  .LocalScale(new Vector3(1, 1, 1))
                  .RectMinMaxZero()
                  .Apply<UIPanel>(p=> 
                  {
                      p.PanelInfo = new UIPanelInfo(uilevel, uidata, panelName);
                      p.OnInit(uidata);
                      mLoadPanels.Add(panelName, p);
                     
                  });

                }
               
            });
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
                mLoadPanels.Remove(panel.PanelInfo.mPanelName);
                panel.OnClose();
            }
               
        }

        public void PushPanel<T>()where T:IPanel
        {
           IPanel panel=GetPanel<T>();
            if (panel != null)
                PushPanel(panel);
        }

        public void BackPanel(string curPanelName=null)
        {
            if (!string.IsNullOrEmpty(curPanelName))
                ClosePanel(curPanelName);

            if(mStacks!=null && mStacks.Count > 0)
            {
                var uiPanelData = mStacks.Pop();
                OpenPanel(uiPanelData.mPanelName, uiPanelData.mPanelLevel, uiPanelData.mPanelData);
            }
           
        }
    }

    
}
