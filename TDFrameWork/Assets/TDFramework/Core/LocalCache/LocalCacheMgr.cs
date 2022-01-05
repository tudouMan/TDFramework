using System;
using UnityEngine;

namespace TDFramework.Cache
{

    public class LocalCacheMgr:ManagerBase,IDisposable
    {

        public TestLocalCache TestLocalCache;
      
        public void Save()
        {
            LocalCacheUtil.Write<TestLocalCache>(TestLocalCache);
            GameEntry.Debug.Log("Save LocalCache Done");
        }

        internal override void Init()
        {
           TestLocalCache = LocalCacheUtil.Read<TestLocalCache>();
        }

        public void Dispose()
        {

        }
    }



} 
