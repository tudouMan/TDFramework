using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.HeapPool
{
    public interface IHeapObject
    {
        void OnInit(object[] parmas=null);

        void Pop(object[] parmas = null);

        void Push();
    }
}
