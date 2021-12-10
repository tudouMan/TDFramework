using System.IO;
using ILRuntime.Runtime.Enviorment;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System.Reflection;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using System.Collections.Generic;

namespace TDFramework.Runtime
{
    public class ILRuntimeMgr:MonoBehaviour
    {
        //大家在正式项目中请全局只创建一个AppDomain
        private AppDomain m_Appdomain;

        public AppDomain MAppdomain { get => m_Appdomain; set => m_Appdomain = value; }

 

        void InitializeILRuntime()
        {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            m_Appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            //这里做一些ILRuntime的注册
            m_Appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            m_Appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());

            SetupCLRRedirection();
            SetupCLRRedirection2();
        }


        public async void RuntimeFunc(string dllName,string pdbName,Action callBack)
        {

            TextAsset dllTex = await Addressables.LoadAssetAsync<TextAsset>(dllName).Task;
            TextAsset pdbTex = await Addressables.LoadAssetAsync<TextAsset>(pdbName).Task;

            byte[] dll = dllTex.bytes;
            byte[] pdb = pdbTex.bytes;
            var fs = new MemoryStream(dll);
            var p = new MemoryStream(pdb);
            try
            {
                m_Appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch
            {
                Debug.LogError("加载热更DLL失败，请确保已经通过VS编译过热更DLL");
            }

            Debug.Log("加载完毕");

            callBack?.Invoke();

            // m_Appdomain.Invoke("Game.HotManager", "Debug", null,null);

            IType t = m_Appdomain.LoadedTypes["Game.HotManager"];
            Type type = t.ReflectionType;

            object instance = m_Appdomain.Instantiate("Game.HotManager");


            MethodInfo mi = type.GetMethod("Debug");
            object getReturn=  mi.Invoke(instance,new object[1] { 3});
            Debug.Log(getReturn.ToString());
            

        }

        internal  void Init()
        {
            m_Appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            
            InitializeILRuntime();
        }

        public void Dispose()
        {
            
        }

        unsafe void SetupCLRRedirection()
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
                {
                    m_Appdomain.RegisterCLRMethodRedirection(i, AddComponent);
                }
            }
        }

        unsafe void SetupCLRRedirection2()
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    m_Appdomain.RegisterCLRMethodRedirection(i, GetComponent);
                }
            }
        }

        MonoBehaviourAdapter.Adaptor GetComponent(ILType type)
        {
            var arr = GetComponents<MonoBehaviourAdapter.Adaptor>();
            for (int i = 0; i < arr.Length; i++)
            {
                var instance = arr[i];
                if (instance.ILInstance != null && instance.ILInstance.Type == type)
                {
                    return instance;
                }
            }
            return null;
        }

        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.AddComponent(type.TypeForCLR);
                }
                else
                {
                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                                                                               //接下来创建Adapter实例
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    clrInstance.ILInstance = ilInstance;
                    clrInstance.AppDomain = __domain;
                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance

                    clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.GetComponent(type.TypeForCLR);
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
                            if (clrInstance.ILInstance.Type == type)
                            {
                                res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }
    }
}
