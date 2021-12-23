using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Table
{
    public class TableDataManager : ManagerBase, IDisposable
    {
        //test
        public DataDataTable m_TestTable;

        //当前内存中加载Table Count
        private int m_TableCount;

        public int TableCount { get => m_TableCount; set => m_TableCount = value; }

        /// <summary>
        /// Addressable 中Table 标签名
        /// </summary>
        public readonly string ResTableName = "Table";

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
          //  m_TestTable = new DataDataTable();
        }

       
    }
}
