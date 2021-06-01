using System;
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

        //显示在Inspector数据字典
#if UNITY_EDITOR
        private Dictionary<Type, int> m_ClassObjInspectorDic=new Dictionary<Type, int>();
#endif

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
               int hash=typeof(T).GetHashCode();
                Stack<object> stack = null;
                m_ClassObjDic.TryGetValue(hash, out stack);
                if (stack == null)
                {
                    stack = new Stack<object>();
                    m_ClassObjDic[hash]= stack;
                }
                    
                if (stack.Count > 0)
                {
                    object obj=stack.Pop();
#if UNITY_EDITOR
                    Type t = obj.GetType();
                    if (m_ClassObjInspectorDic.ContainsKey(t))
                        m_ClassObjInspectorDic[t]--;
                    else
                        m_ClassObjInspectorDic[t]=0;
                    return (T)obj;
#endif
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

                stack.Push(obj);

#if UNITY_EDITOR
                Type t = obj.GetType();
                if (m_ClassObjInspectorDic.ContainsKey(t))
                    m_ClassObjInspectorDic[t]++;
                else
                    m_ClassObjInspectorDic[t] = 1;
#endif
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

#if UNITY_EDITOR
                        t = obj.GetType();
                        if (m_ClassObjInspectorDic.ContainsKey(t))
                            m_ClassObjInspectorDic[t]--;
                        else
                            throw new Exception("pool remove class inspector dic error not key:");
#endif
                    }

#if UNITY_EDITOR
                    if (stackTotal == 0)
                    {
                        if (t != null)
                            m_ClassObjInspectorDic.Remove(t);

                    }
#endif
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
