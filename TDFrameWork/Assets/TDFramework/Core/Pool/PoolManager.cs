using System;
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
    }
}
