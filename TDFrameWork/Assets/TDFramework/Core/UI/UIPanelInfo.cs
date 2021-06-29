using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.UI
{
    public enum PanelState
    {
        Null,
        Init,
        Open,
        Show,
        Hide,
        Close,
    }

    public class UIPanelInfo
    {
        public UILevel m_PanelLevel;  //层级
        public IUIData m_PanelData; //UI数据
        public string m_PanelName;  //UI名字
        public string m_PanelAssetLoadName; //UI加载路径
        public PanelState m_State;
        public UIPanelInfo(UILevel level,IUIData data,string panelName,string assetName=null)
        {
            m_PanelLevel = level;
            m_PanelData = data;
            m_PanelName = panelName;
            m_PanelAssetLoadName = assetName;
        }
        public UIPanelInfo()
        {

        }
    }
}
