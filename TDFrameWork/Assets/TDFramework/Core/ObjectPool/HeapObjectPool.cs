using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDFramework.HeapPool
{
    public class HeapScriptsPool<T>where T:IHeapObject,new ()
    {
        private Queue<T> mQueue;

        private int mPoolSize;

        public HeapScriptsPool(int size=2)
        {
            mQueue = new Queue<T>();
            mPoolSize = size;
        }

        public int CurCount
        {
            get
            {
                return mQueue == null ? 0 : mQueue.Count;
            }
        }

        public bool IsPoolSizeFull
        {
            get
            {
                return mQueue.Count > mPoolSize;
            }
        }

        public T Pop(object[] parmas = null)
        {
            T obj;
            if (mQueue.Count > mPoolSize)
            {
                obj = mQueue.Dequeue();
            }  
            else
            {
                obj = new T();
                obj.OnInit(parmas);
            }
            if (obj != null)
                obj.Pop(parmas);
            else
                throw new Exception(typeof(T).Name + "pool Pop is error,obj not exsit");

            return obj;
        }

        public void Push(T obj)
        {
            if(obj==null)
                throw new Exception(typeof(T).Name + "pool Push is error,obj not have");

            obj.Push();

            mQueue.Enqueue(obj);
        }

    }




    public class HeapGameObjectPool<T>where T : UnityEngine.Object,IHeapObject
    {
        private Queue<T> mQueue;
        private int mPoolSize;
        private GameObject mPrefab;

        public bool IsPoolSizeFull
        {
            get
            {
                return mQueue.Count > mPoolSize;
            }
        }


        public int CurCount
        {
            get
            {
                return mQueue == null ? 0 : mQueue.Count;
            }
        }

        public HeapGameObjectPool(GameObject _prefabObj,int size)
        {
           if(_prefabObj==null)
                throw new Exception(typeof(T).Name + "pool prefab is error,obj not exsit");
            mQueue = new Queue<T>();
            mPrefab = _prefabObj;
            mPoolSize = size;
        }



        public T Pop(object[] parmas = null)
        {
            T obj;
            if (mQueue.Count > mPoolSize)
            {
                obj = mQueue.Dequeue();
            }
            else
            {
               GameObject instancePrefab=GameObject.Instantiate(mPrefab);
               obj = instancePrefab.GetComponent<T>();
               obj.OnInit(parmas);
            }

            if (obj != null)
                obj.Pop(parmas);
            else
                throw new Exception(typeof(T).Name + "pool Pop is error,obj not exsit");

            return obj;
        }

        public void Push(T obj)
        {
            if (obj == null)
                throw new Exception(typeof(T).Name + "pool Push is error,obj not have");

            obj.Push();
            mQueue.Enqueue(obj);
        }

        public void ReleasePool()
        {
            while (mQueue.Count > 0)
            {
                GameObject.Destroy(mQueue.Dequeue());
            }
        }
    }
}
