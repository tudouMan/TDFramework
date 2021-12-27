using System;
using System.Collections.Generic;


namespace TDFramework
{
    public class TimeManager : ManagerBase, IDisposable
    {
        private LinkedList<TimeAction> m_TimeActions;

        internal override void Init()
        {
            m_TimeActions = new LinkedList<TimeAction>();
        }


        public void Register(TimeAction action)
        {
            m_TimeActions.AddLast(action);
        }

        public void Remove(TimeAction action,bool isCompleteEvent=false)
        {
            if (action == null)
                throw new Exception("action is null");
            if(isCompleteEvent)
                action.CompleteAction?.Invoke();
            m_TimeActions.Remove(action);
            GameEntry.Pool.PushClass<TimeAction>(action);
        }


        public void RemoveByTimeName(string name)
        {
            LinkedListNode<TimeAction> cur = m_TimeActions.First;
            while (cur != null)
            {
                if (cur.Value.Name.Equals(name,StringComparison.CurrentCultureIgnoreCase))
                {
                    Remove(cur.Value);
                    break;
                   
                }

                cur = cur.Next;
            }
        }


        public TimeAction CreatTimeAction()
        {
            return GameEntry.Pool.PopClass<TimeAction>();
        }


        public void Update()
        {
            for (LinkedListNode<TimeAction>cur=m_TimeActions.First;cur!=null;cur=cur.Next)
            {
                if(cur.Value.StartAction!=null && (cur.Value.StartAction.Target==null || cur.Value.StartAction.Target.ToString() == "null"))
                {
                    cur.Value.Stop();
                    continue;
                }

                if(cur.Value.RunAction!=null && (cur.Value.RunAction.Target==null || cur.Value.RunAction.Target.ToString() == "null"))
                {
                    cur.Value.Stop();
                    continue;
                }


                if(cur.Value.CompleteAction!=null && (cur.Value.CompleteAction.Target==null || cur.Value.CompleteAction.Target.ToString() == "null"))
                {
                    cur.Value.Stop();
                    continue;
                }

                cur.Value.Update();
            }
        }

        public void Dispose()
        {
            GameEntry.Debug.Log("TimeManager Dispose");
            m_TimeActions.Clear();
        }


        /// <summary>
        /// 改变TimeScale
        /// </summary>
        /// <param name="scale">scale值</param>
        /// <param name="continueTime">持续时间</param>
        public void ChangeTimeScale(float scale,float continueTime)
        {
            UnityEngine.Time.timeScale = scale;
            CreatTimeAction().Init(null, continueTime, 0, 0,null,null,
                ()=>
                {
                    UnityEngine.Time.timeScale = 1;
                })
                .Run();
        }

       
    }
}
