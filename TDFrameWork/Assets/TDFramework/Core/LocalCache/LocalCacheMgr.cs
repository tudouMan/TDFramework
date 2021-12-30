using System;
using UnityEngine;

namespace TDFramework.Cache
{

    public class LocalCacheMgr:ManagerBase,IDisposable
    {

        public TestLocalCache TestLocalCache;
        public void Dispose()
        {
             
        }

        public void Save()
        {
            GameEntry.Debug.Log("Save LocalCache");
            LocalCacheUtil.Write<TestLocalCache>(TestLocalCache);
        }

        internal override void Init()
        {
            GameEntry.Debug.Log("Init LocalCache");
            TestLocalCache = LocalCacheUtil.Read<TestLocalCache>();
        }
    }
} 
