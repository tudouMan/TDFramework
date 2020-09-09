using UnityEngine;
using System.Collections;
using System.IO;
using ILRuntime.Runtime.Enviorment;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System.Linq;
using TDFramework.Resource;

public class HelloWorld : MonoBehaviour
{
    
    //大家在正式项目中请全局只创建一个AppDomain
    AppDomain appdomain;

    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;


    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "get"))
        {
            ILruntimeTest();
        }
    }

    async void ILruntimeTest()
    {
        //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

        InitializeILRuntime();

        TextAsset dllTex=await Addressables.LoadAssetAsync<TextAsset>("HotFix_Project.dll").Task;
        TextAsset pdbTex = await Addressables.LoadAssetAsync<TextAsset>("HotFix_Project.pdb").Task;

        byte[] dll = dllTex.bytes;
        byte[] pdb = pdbTex.bytes;


        fs = new MemoryStream(dll);
        p = new MemoryStream(pdb);
        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("加载热更DLL失败，请确保已经通过VS打开Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln编译过热更DLL");
        }

       
        OnHotFixLoaded();
    }



    void InitializeILRuntime()
    {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    void OnHotFixLoaded()
    {
        //HelloWorld，第一次方法调用
        appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);

    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
        if (p != null)
            p.Close();
        fs = null;
        p = null;
    }

    void Update()
    {
       
    }
}

