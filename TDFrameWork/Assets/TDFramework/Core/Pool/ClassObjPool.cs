﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Pool
{
    public class ClassObjPool : IDisposable
    {
        //类型设定个数
        private Dictionary<int, int> m_ClassObjCountDic;

        //类型字典
        private Dictionary<int, Stack<object>> m_ClassObjDic;



        public ClassObjPool()
        {
            m_ClassObjCountDic = new Dictionary<int, int>();
            m_ClassObjDic = new Dictionary<int, Stack<object>>();
        }

        public void SetClassObjCount<T>(int count)where T : class
        {
            int hash= typeof(T).GetHashCode();
            m_ClassObjCountDic[hash] = count;
        }

        public T Pop<T>()where T:class,new()
        {
            lock (m_ClassObjDic)
            {
                int hash = typeof(T).GetHashCode();
                Stack<object> stack = null;
                m_ClassObjDic.TryGetValue(hash, out stack);
                if (stack == null)
                {
                    stack = new Stack<object>();
                    m_ClassObjDic[hash] = stack;
                }

                if (stack.Count > 0)
                {
                    return stack.Pop() as T;
                }
                else
                {
                    return new T();
                }
            }
        
            
        }

        public void Push<T>(object obj)where T:class,new()
        {
            lock (m_ClassObjDic)
            {
                int hash=typeof(T).GetHashCode();
               

                Stack<object> stack = null;
                m_ClassObjDic.TryGetValue(hash, out stack);
                if (stack == null)
                {
                    stack = new Stack<object>();
                    m_ClassObjDic[hash] = stack;
                }
                else
                {
                    if (m_ClassObjCountDic.ContainsKey(hash) && stack.Count>= m_ClassObjCountDic[hash])
                    {
                        obj = null;
                        return;
                    }
                }

                stack.Push(obj);


            }
        }


        public void ReleasePool()
        {
            lock (m_ClassObjDic)
            {
               var enumerator=m_ClassObjDic.GetEnumerator();
                Type t = null;
                while (enumerator.MoveNext())
                {
                    int key=enumerator.Current.Key;
                    Stack<object>stack=enumerator.Current.Value;
                    int stackTotal = stack.Count;
                    int setTotal = m_ClassObjCountDic[key];
                    while (stackTotal > setTotal)
                    {
                        object obj=stack.Pop();
                        stackTotal--;
                    }

                }

                GC.Collect();
            }
        }

        public void Dispose()
        {
            m_ClassObjDic.Clear();
        }
    }
}
