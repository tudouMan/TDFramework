using System;
using System.Collections.Generic;

namespace TDFramework.Pool
{
    public class PrefabPool : IDisposable
    {
        private Dictionary<int, PrefabPoolEntity> poolDic;

        private Dictionary<int, int> poolInspectorDic;

        public PrefabPool()
        {
            poolDic = new Dictionary<int, PrefabPoolEntity>();
#if UNITY_EDITOR
            poolInspectorDic = new Dictionary<int, int>();
#endif
        }


        public void Pop(string prefabName, Action<UnityEngine.GameObject> complete)
        {

            
            PrefabPoolEntity pool = null;
            int prefabId= AddressableManager.GetPrefabId(prefabName);
            poolDic.TryGetValue(prefabId, out pool);
            if (pool == null)
            {
                pool = new PrefabPoolEntity();
                pool.PoolName = prefabName;
                pool.PrefabId= prefabId;
                pool.Stack = new Stack<UnityEngine.GameObject>();
                poolDic[prefabId] = pool;
                
            }

           
            if (pool.Stack.Count > 0)
            {
               UnityEngine.GameObject prfab=pool.Stack.Pop();
               complete?.Invoke(prfab);
#if UNITY_EDITOR
                if (poolInspectorDic.ContainsKey(prefabId))
                    poolInspectorDic[prefabId]--;
                else
                    poolInspectorDic[prefabId] =0;
#endif
            }
            else
            {
                UnityEngine.GameObject  loadPrefab= AddressableManager.InstanceObj(prefabName);
                complete?.Invoke(loadPrefab);
            }

        }


        public void Push(UnityEngine.GameObject prefab)
        {
            int prefabId=prefab.GetInstanceID();
            PrefabPoolEntity pool = null;
            poolDic.TryGetValue(prefabId, out pool);
            if (pool == null)
            {
                pool = new PrefabPoolEntity();
                pool.Stack = new Stack<UnityEngine.GameObject>();
            }

#if UNITY_EDITOR
            if (poolInspectorDic.ContainsKey(prefabId))
                poolInspectorDic[prefabId]++;
            else
                poolInspectorDic[prefabId] = 1;
#endif
             //Todo进行细分挂显示具体信息
            //prefab.transform.parent = GameEntry.Instance.transform;
            pool.Stack.Push(prefab);
        }



        public void Dispose()
        {
            poolDic.Clear();
        }
    }


    //TODO
    public class AddressableManager
    {
        public static int GetPrefabId(string name)
        {
            return 1000;
        }
        public static UnityEngine.GameObject LoadGameObject(string path)
        {
            return new UnityEngine.GameObject();
        }

        public static UnityEngine.GameObject InstanceObj(string path)
        {
            return new UnityEngine.GameObject();
        }

        public static UnityEngine.GameObject LoadGameObject(string path,BaseAction action)
        {
            return new UnityEngine.GameObject();
        }

    }
}
