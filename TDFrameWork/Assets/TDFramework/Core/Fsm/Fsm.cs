﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDFramework
{
    public class Fsm<T>:FsmBase where T:class
    {
      
        public T Owner { get;private set; }

        private Dictionary<int, FsmState<T>> m_StateDic;
        
        private FsmState<T> m_CurState;

        public Fsm(T owner,int fsmId, FsmState<T>[]fsmStates):base(fsmId: fsmId)
        {
            m_StateDic = new Dictionary<int, FsmState<T>>();
           
            Owner = owner;
            for (int i = 0; i < fsmStates.Length; i++)
            {
                FsmState<T> state = fsmStates[i];
                state.Fsm = this;
                m_StateDic.Add(i, state);
                GameEntry.Debug.Log("I:"+i.ToString()+"  TYPE:"+state.GetType().ToString());
            }
            m_CurState = fsmStates[0];
            CurStateType = -1;
        }


        public void EnterState(int stateType)
        {
            FsmState<T> state = null;
            m_StateDic.TryGetValue(stateType, out state);
            if (state == null)
                GameEntry.Logger.Write($"not this type num:{stateType} state,please check", UnityEngine.LogType.Exception);
              

            if (state == m_CurState) 
                return;

            m_CurState.OnExit();
            m_CurState = state;
            m_CurState.OnEnter();
        }


        public FsmState<T> GetState(int stateType)
        {
            FsmState<T> state = null;
            m_StateDic.TryGetValue(stateType, out state);
            if (state == null)
               GameEntry.Debug.LogWarning($"id{FsmId.ToString()} not this type{stateType.ToString()} state");

            return state;
        }


        public override void Update()
        {
            if (m_CurState != null)
                m_CurState.OnUpdate();
        }
         
        public override void ShutDown()
        {
            if (m_CurState != null)
                m_CurState.OnExit();

            m_CurState = null;
            foreach (KeyValuePair<int,FsmState<T>>state in m_StateDic)
            {
                state.Value.OnDestory();
            }

            m_StateDic.Clear();
        }
    }
}
