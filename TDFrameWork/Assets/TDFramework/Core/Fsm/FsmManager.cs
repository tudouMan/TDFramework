using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDFramework
{
    public class FsmManager : ManagerBase, IDisposable
    {
        private Dictionary<int, FsmBase> m_FsmDic;

        private int m_FsmTempId;


        internal override void Init()
        {
            m_FsmDic = new Dictionary<int, FsmBase>();
        }

        public Fsm<T> CreatFasm<T>(T owner, FsmState<T>[] states)where T:class
        {
            m_FsmTempId++;
            Fsm<T> fsm = new Fsm<T>(owner, m_FsmTempId,states);
            m_FsmDic.Add(m_FsmTempId,fsm);
            return fsm;
        }


        public void ShutDownFsm(int fsmId)
        {
            FsmBase fsm = null;
            m_FsmDic.TryGetValue(fsmId, out fsm);
            if (fsm != null)
            {
                fsm.ShutDown();
                m_FsmDic.Remove(fsmId);
            }
               
        }

        public void Update()
        {
     
            var enumerator = m_FsmDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                 enumerator.Current.Value.Update();
            }
           
        }
  

        public void Dispose()
        {
            var enumerator = m_FsmDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.ShutDown();
            }

            m_FsmDic.Clear();
        }
    }
}
