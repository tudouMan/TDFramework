using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Cache
{

    public class LocalCacheMgr:ManagerBase,IDisposable
    {
        public void Dispose()
        {
            Save();
        }

        public void Save()
        {
            GameEntry.Debug.Log("Save LocalCache");
        }

        internal override void Init()
        {
           
        }
    }
}
