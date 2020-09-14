using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.StateMachine
{
    /// <summary>
    /// 状态工厂
    /// </summary>
    public class StateFactory
    {
        private Dictionary<string, IState> stateDic = new Dictionary<string, IState>();

        private Stack<IState> stateStack = new Stack<IState>();

        public IState GetState(string stateNmae)
        {
            IState resultState = null;
            if (stateDic.TryGetValue(stateNmae, out resultState))
                return resultState;
            else
                throw new Exception("stateDic not has this state:" + stateNmae);

        }

        public IState ToState(string stateNmae)
        {
            if (stateStack.Count > 0)
            {
               IState popState= stateStack.Pop();
                popState.OnExit();
            }
            IState getState= GetState(stateNmae);
            getState.OnEnter();
            stateStack.Push(getState);
            return getState;
        }


        public ClassEnumerator RegisterClassAttribute<TAttribute>(Assembly assembly)where TAttribute:GameStateAttribute
        {
            ClassEnumerator classEnumerator = new ClassEnumerator(typeof(TAttribute), typeof(IState), assembly);
            var results = classEnumerator.AllTypes.GetEnumerator();
            while (results.MoveNext())
            {
                var cur = results.Current;
                IState state = (IState)System.Activator.CreateInstance(cur);
                string typeName = state.GetType().Name;
                if (!stateDic.ContainsKey(typeName))
                    stateDic.Add(typeName, state);
            }
            return classEnumerator;
        }
    }
}
