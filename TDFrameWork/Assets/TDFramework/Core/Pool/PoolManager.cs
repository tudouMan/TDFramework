﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Pool
{
    public class PoolManager : ManagerBase, IDisposable
    {
        private ClassObjPool m_ClassPool;
        private PrefabPool m_PrefabPool;

        public ClassObjPool ClassPool { get => m_ClassPool; set => m_ClassPool = value; }
        public PrefabPool PrefabPool { get => m_PrefabPool; set => m_PrefabPool = value; }

        internal override void Init()
        {
            m_ClassPool = new ClassObjPool();
            m_PrefabPool = new PrefabPool();
        }

        public void Dispose()
        {
            m_ClassPool.Dispose();
            m_PrefabPool.Dispose();
        }

        #region ClassPool
        public void SetClassObjCount<T>(int count) where T : class
        {
            m_ClassPool.SetClassObjCount<T>(count);
        }

        public T PopClass<T>() where T : class, new()
        {
            return m_ClassPool.Pop<T>();
        }

        public void PushClass<T>(object obj)where T:class,new ()
        {
            m_ClassPool.Push<T>(obj);
        }

        public void ReleaseClassPool()
        {
            m_ClassPool.ReleasePool();
        }

        #endregion

        #region PrefabPool
        public void PopPrefab(string prefabName, Action<UnityEngine.GameObject> completeCallBack)
        {
            m_PrefabPool.Pop(prefabName, completeCallBack);
        }

       

        public void PushPrefab(UnityEngine.GameObject prefab)
        {
            m_PrefabPool.Push(prefab);
        }
        #endregion
    }
}
