using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework
{
    public class TimeAction
    {
    
        public string Name
        {
            get;private set;
        }
        public bool IsRunning
        {
            get;private set;
        }

        //是否暂停
        private bool m_IsPause;

        //当前时间
        private float m_CurRunTime;

        //延迟时间
        private float m_DelayTime;

        //周期时间
        private float m_Interval;

        //循环次数
        //-1 永远循环, 0-loops 0也会执行一次
        private int m_Loop;

        //当前循环次数
        private int m_CurLoop;

        //上一次暂停时间
        private float m_LastPauseTime;

        //暂停时间
        private float m_Pausetime;

        /// <summary>
        /// 开始行为
        /// </summary>
        public Action StartAction { get; private set; }

        /// <summary>
        /// 循环一次行为 返回循环次数
        /// </summary>
        public Action<int> RunAction { get; private set; }

        /// <summary>
        /// 结束行为
        /// </summary>
        public Action CompleteAction { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="timeName">名字</param>
        /// <param name="delaytime">延迟时间</param>
        /// <param name="interval">周期时间</param>
        /// <param name="loopType"></param>
        /// <param name="startAction">开始行为</param>
        /// <param name="runAction">循环一次行为</param>
        /// <param name="completeAction">结束行为</param>
        /// <returns></returns>
        public TimeAction Init(string timeName=null,float delaytime=0,float interval=0,int loopType=1,Action startAction=null,Action<int>runAction=null,Action completeAction=null)
        {

            m_DelayTime = delaytime;
            m_Interval = interval;
            m_Loop = loopType;
            StartAction = startAction;
            RunAction = runAction;
            CompleteAction = completeAction;
            return this;
        }


        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            m_IsPause = false;
            m_CurRunTime = UnityEngine.Time.realtimeSinceStartup;
            m_CurLoop = 0;
            //添加到控制器中 
            GameEntry.Time.Register(this);
          
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
            //移除控制器
            GameEntry.Time.Remove(this);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            m_LastPauseTime = UnityEngine.Time.realtimeSinceStartup;
            m_IsPause = true;

        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            m_IsPause = false;
            m_Pausetime = UnityEngine.Time.realtimeSinceStartup - m_LastPauseTime;

        }

        public void Update()
        {
            if (m_IsPause) 
                return;

            
            //计算delay 时间
            if (UnityEngine.Time.realtimeSinceStartup >=m_CurRunTime + m_DelayTime+m_LastPauseTime)
            {
                if (!IsRunning)
                {
                    m_CurRunTime = UnityEngine.Time.realtimeSinceStartup;
                    m_DelayTime = 0;
                    StartAction?.Invoke();
                }

                IsRunning = true;
            }



            if (!IsRunning)
                return;

            //计算interval
            if (UnityEngine.Time.realtimeSinceStartup >= m_CurRunTime + m_Pausetime)
            {
                m_CurRunTime = UnityEngine.Time.realtimeSinceStartup + m_Interval;
                m_Pausetime = 0;


                RunAction?.Invoke(m_Loop - m_CurLoop);
                //Loop即使为0也执行一次
                if (m_Loop != -1)
                {
                    if (m_CurLoop >= m_Loop)
                    {
                        CompleteAction?.Invoke();
                        Stop();
                    }

                    m_CurLoop++;
                }

            }

        }
    }
}
