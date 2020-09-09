using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Cache
{
    [MonoSingletonPath("[Cache]/[本地缓存]")]
    public class LocalCacheMgr : MonoSingleton<LocalCacheMgr>
    {
        //public TempCache mTempCache;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
           // mTempCache = LocalCacheUtil.Read<TempCache>();

        }


        public void Save()
        {
           // mTempCache.mLevel++;
           // LocalCacheUtil.Write(mTempCache);
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Save();
        }
    }
}
