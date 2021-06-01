using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework
{
    public class FsmBase
    {
        public int FsmId { get; private set; }

        public int CurStateType { get; set; }

        public FsmBase(int fsmId)
        {
            FsmId = fsmId;
        }

        public virtual void ShutDown() { }
    }
}
