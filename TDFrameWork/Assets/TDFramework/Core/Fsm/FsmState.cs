using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework
{
    public abstract  class  FsmState<T> where T:class
    {
        public Fsm<T> Fsm;

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();

        //destory event
        public abstract void OnDestory();
    }
}
