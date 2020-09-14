using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.StateMachine
{

    /// <summary>
    /// 获取到该程序集中带Attribute类型iterfaceType(排除基类)
    /// </summary>
    public class ClassEnumerator
    {
        private List<Type> allTypes = new List<Type>();
        public List<Type> AllTypes { get { return allTypes; } }

        public ClassEnumerator(Type attributeType,Type iterfaceType,Assembly assemblyType)
        {
            Type[] types = assemblyType.GetTypes();
            if (types == null)
                throw new Exception(string.Format("this {0}Assembly is not types",assemblyType.FullName));
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                //判断类型T是否来自基类
                if (iterfaceType.IsAssignableFrom(t))
                {
                    //判断是否为基类
                    if (!t.IsAbstract)
                    {
                        //通过反射获得自定义特性
                        if (t.GetCustomAttributes(attributeType, false).Length > 0)
                        {
                            allTypes.Add(t);
                        }
                    }
                }
            }
        }
    }
}
