using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Pool
{
    public class PrefabPoolEntity
    {
        public int PrefabId;

        public string PoolName;

        public Stack<UnityEngine.GameObject> Stack;
    }

   
}
