using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TDFramework.Runtime
{
    public class RuntimeMgr : ManagerBase, IDisposable
    {
        private Assembly m_AssemblyRoot;
        private Dictionary<string, Assembly> m_AssemblyDic;
        private const string m_DllName= "dll_res";

        public void Dispose()
        {

        }

        internal override void Init()
        {
            m_AssemblyDic = new Dictionary<string, Assembly>();
            GameEntry.Res.LoadAssetAsync<TextAsset>(m_DllName, p =>
            {
                byte[] bytes = p.bytes;
                m_AssemblyRoot = Assembly.Load(bytes);
            });
        }

        public void RuntimeStaticFunc(string typeName,string methodName,object[]args=null)
        {
            if (m_AssemblyRoot != null)
            {
                Type type = m_AssemblyRoot.GetType(typeName);
                MethodInfo mi = type.GetMethod(methodName);
                mi.Invoke(null, args);

            }
        }

        public void RuntimeFunc(string typeName, string methodName, object[] args = null)
        {
            if (m_AssemblyRoot != null)
            {
                object type = m_AssemblyRoot.CreateInstance(typeName);
                MethodInfo mi = type.GetType().GetMethod(methodName);
                mi.Invoke(type, args);
            }

         
        }
    }
}
