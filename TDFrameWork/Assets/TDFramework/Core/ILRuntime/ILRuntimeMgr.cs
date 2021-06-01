using System.IO;
using ILRuntime.Runtime.Enviorment;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace TDFramework.Runtime
{
    public class ILRuntimeMgr:ManagerBase,IDisposable
    {
        //大家在正式项目中请全局只创建一个AppDomain
        AppDomain mAppdomain;

        public AppDomain MAppdomain { get => mAppdomain; set => mAppdomain = value; }

 

        void InitializeILRuntime()
        {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            mAppdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            //这里做一些ILRuntime的注册
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
                mAppdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch
            {
                Debug.LogError("加载热更DLL失败，请确保已经通过VS编译过热更DLL");
            }

            Debug.Log("加载完毕");

            callBack?.Invoke();
        }

        internal override void Init()
        {
            mAppdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            InitializeILRuntime();
        }

        public void Dispose()
        {
            
        }
    }
}
