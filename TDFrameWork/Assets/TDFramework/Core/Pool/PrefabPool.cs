using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace TDFramework.Pool
{
    public class PrefabPool : IDisposable
    {
        private Dictionary<string, PrefabPoolEntity> m_PoolDic;
        private Dictionary<string, int> m_PoolInspectorDic;
        private Dictionary<int, string> m_PrefabIdDic;

        public Dictionary<string, PrefabPoolEntity> PoolDic { get => m_PoolDic; set => m_PoolDic = value; }
        public Dictionary<string, int> PoolInspectorDic { get => m_PoolInspectorDic; set => m_PoolInspectorDic = value; }
        public Dictionary<int, string> PrefabIdDic { get => m_PrefabIdDic; set => m_PrefabIdDic = value; }

        public PrefabPool()
        {
            m_PoolDic = new Dictionary<string, PrefabPoolEntity>();
            m_PrefabIdDic = new Dictionary<int, string>();
            m_PoolInspectorDic = new Dictionary<string, int>();
        }


        public void Pop(string prefabName, Action<UnityEngine.GameObject> completeCallBack)
        {
            PrefabPoolEntity pool = null;
            m_PoolDic.TryGetValue(prefabName, out pool);
            if (pool == null)
            {
                pool = new PrefabPoolEntity();
                pool.PoolName = prefabName;
                pool.Stack = new Stack<UnityEngine.GameObject>();
                m_PoolDic[prefabName] = pool;
            }

            
            if (pool.Stack.Count > 0)
            {
                UnityEngine.GameObject prefab = pool.Stack.Pop();
                if (!m_PrefabIdDic.ContainsKey(prefab.GetInstanceID()))
                    m_PrefabIdDic.Add(prefab.GetInstanceID(), prefabName);
                else
                    m_PrefabIdDic[prefab.GetInstanceID()] = prefabName;

                if (m_PoolInspectorDic.ContainsKey(prefabName))
                    m_PoolInspectorDic[prefabName] -= 1;
                else
                    GameEntry.Debug.LogError($"Pool {prefabName} InSpectorDic Is Null ");

                completeCallBack?.Invoke(prefab);
            }
            else
            {
                GameEntry.Res.InstanceAsync(prefabName,p=> 
                {
                    completeCallBack?.Invoke(p.gameObject);
                    lock (m_PrefabIdDic)
                    {
                        if (!m_PrefabIdDic.ContainsKey(p.GetInstanceID()))
                        {
                            m_PrefabIdDic.Add(p.GetInstanceID(), prefabName);
                        }
                        else
                        {
                            m_PrefabIdDic[p.GetInstanceID()] = prefabName;
                        }
                          
                    }

                    lock (m_PoolInspectorDic)
                    {
                        if (!m_PoolInspectorDic.ContainsKey(prefabName))
                            m_PoolInspectorDic.Add(prefabName, 0);
                    }
                });
               
            }

        }


        public void Push(UnityEngine.GameObject prefab)
        {
            if (prefab == null)
                throw new Exception("this gameobect is null");

            PrefabPoolEntity pool = null;
            string poolName = string.Empty;
            m_PrefabIdDic.TryGetValue(prefab.GetInstanceID(),out poolName);

            if (string.IsNullOrEmpty(poolName))
            {
                GameEntry.Debug.LogError($"not this {prefab.GetInstanceID().ToString()} instanceId prefab");
                return;
            }
                
            m_PoolDic.TryGetValue(poolName, out pool);

            if (pool == null)
            {
                GameEntry.Debug.Log($"push this {poolName} pool,pool is not initialize ");
                pool = new PrefabPoolEntity();
                pool.Stack = new Stack<UnityEngine.GameObject>();
            }

            pool.Stack.Push(prefab);

            m_PrefabIdDic.Remove(prefab.GetInstanceID());

            if (m_PoolInspectorDic.ContainsKey(poolName))
                m_PoolInspectorDic[poolName] -= 1;
            

            GameEntry.Debug.Log($"{poolName}pool size is {pool.Stack.Count.ToString()}");
        }



        public void Dispose()
        {
            m_PoolDic.Clear();
        }
    }


   
}
