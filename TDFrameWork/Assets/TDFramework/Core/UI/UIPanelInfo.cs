using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.UI
{
    public class UIPanelInfo
    {
        public UILevel mPanelLevel;  //层级
        public IUIData mPanelData; //UI数据
        public string mPanelName;  //UI名字
        public string mPanelAssetLoadName; //UI加载路径
        public UIPanelInfo(UILevel level,IUIData data,string panelName,string assetName=null)
        {
            mPanelLevel = level;
            mPanelData = data;
            mPanelName = panelName;
            mPanelAssetLoadName = assetName;
        }
        public UIPanelInfo()
        {

        }
    }
}
