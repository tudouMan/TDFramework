using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.StateMachine 
{ 
    /// <summary>
    /// 状态基类
    /// </summary>
   public abstract class StateBase:IState
    {
        public virtual void OnEnter() { }
        public virtual void OnUpdate(object args) { }
        public virtual void OnExit() { }
    }

    public interface IState
    {
        void OnEnter();
        void OnUpdate(object args);
        void OnExit();
    }
}
