using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace TDFramework.Pool
{
    public class PrefabPool : IDisposable
    {
        private Dictionary<string, PrefabPoolEntity> poolDic;

        private Dictionary<int, int> poolInspectorDic;
        private Dictionary<int, string> prefabIdDic;

        public PrefabPool()
        {
            poolDic = new Dictionary<string, PrefabPoolEntity>();
            prefabIdDic = new Dictionary<int, string>();
        }


        public void Pop(string prefabName, Action<UnityEngine.GameObject> completeCallBack)
        {
            PrefabPoolEntity pool = null;
            poolDic.TryGetValue(prefabName, out pool);
            if (pool == null)
            {
                pool = new PrefabPoolEntity();
                pool.PoolName = prefabName;
                pool.Stack = new Stack<UnityEngine.GameObject>();
                poolDic[prefabName] = pool;
                
            }

           
            if (pool.Stack.Count > 0)
            {
               UnityEngine.GameObject prfab=pool.Stack.Pop();
                completeCallBack?.Invoke(prfab);
            }
            else
            {
                GameEntry.Res.InstanceAsync(prefabName,p=> 
                {
                    completeCallBack?.Invoke(p.gameObject);
                    lock (prefabIdDic)
                    {
                        if (!prefabIdDic.ContainsKey(p.GetInstanceID()))
                            prefabIdDic.Add(p.GetInstanceID(), prefabName);
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
            prefabIdDic.TryGetValue(prefab.GetInstanceID(),out poolName);

            if (string.IsNullOrEmpty(poolName))
            {
                GameEntry.Debug.LogError($"not this {prefab.GetInstanceID().ToString()} instanceId prefab");
                return;
            }
                
            poolDic.TryGetValue(poolName, out pool);
            if (pool == null)
            {
                GameEntry.Debug.Log($"push this {poolName} pool,pool is not initialize ");
                pool = new PrefabPoolEntity();
                pool.Stack = new Stack<UnityEngine.GameObject>();
            }
            pool.Stack.Push(prefab);

            GameEntry.Debug.Log($"{poolName}pool size is {pool.Stack.Count.ToString()}");
        }



        public void Dispose()
        {
            poolDic.Clear();
        }
    }


   
}
