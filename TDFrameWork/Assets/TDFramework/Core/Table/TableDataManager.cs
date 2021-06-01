using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Table
{
    public class TableDataManager : ManagerBase, IDisposable
    {

        private readonly string m_DataBundlePath =UnityEngine.Application.streamingAssetsPath+"/Data";

        //当前内存中加载Table Count
        private int m_TableCount;

        public int TableCount { get => m_TableCount; set => m_TableCount = value; }



        public void Dispose()
        {
            m_TableCount = 0;

        }

        internal override void Init()
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            //Load Bundle
            //Load Data
            LoadData();
        }

        private void LoadData()
        {
           
        }

      

    }
}
