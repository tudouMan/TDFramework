using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TDFramework;

public enum eChannel
{
    Google = 0,
    TapTap = 1,
    Apple = 2,
    Windos=3
}
public class BuildAPK
{

    [MenuItem("TDFramework/一键打包/VersionStyle")]
    public static void CreatVersionStyle()
    {
        var s = VersionStyle.Instance;
        Selection.activeObject = s;
    }


    public static void SetJenkins()
    {
        resetStyle();
        VersionStyle style = VersionStyle.Instance;
        string dir = Application.dataPath + "/../../";
        string url = dir + "jenkins.txt";
        if (File.Exists(url))
        {
            string info = File.ReadAllText(url);
            Debug.Log("SetJenkins:" + info);
            string[] ps = info.Split('|');
            style.JobName = ps[2].Trim();
            string channel = ps[0].Trim();//google taptap
            if (channel == "taptap")
            {
                style.Channel = eChannel.TapTap;
            }
            else if(channel == "Apple")
            {
                style.Channel = eChannel.Apple;
            }
            string type = ps[1].Trim();//build_il2cpp_GM  build_aab build_il2cpp build1
           
            if (type == "build_GM")
            {
                style.IsGM = true;
                Debug.Log(type);
            }
            else if (type == "build_aab")
            {
                EditorUserBuildSettings.buildAppBundle = true;
                Debug.Log(type);
            }
        }

        style.Save();
        AssetDatabase.Refresh();
    }


    private static List<string> SceneNames = new List<string> 
    {
      "Assets/Scenes/SampleScene.unity"
    };

 
    public static void InitScenes()
    {
        List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>();
        foreach (var item in SceneNames)
        {
            list.Add(new EditorBuildSettingsScene(item, true));
        }
        EditorBuildSettings.scenes = list.ToArray();
    }

    public static string ApkPath = Application.dataPath + "/../../build/";
    

    [MenuItem("TDFramework/一键打包/Build_GooglePlay/il2cpp")]
    public static void Build_GooglePlay()
    {
        resetStyle();
        BuildAndroid();
    }

    [MenuItem("TDFramework/一键打包/BuildWindows")]
    public static void Build_WindowsPlay()
    {
        resetStyle();
        BuildWindows();
    }

    [MenuItem("TDFramework/一键打包/Build_GooglePlay/il2cpp_GM")]
    public static void Build_GooglePlay_GM()
    {
        resetStyle();
        VersionStyle.Instance.IsGM = true;
        BuildAndroid();
    }



    [MenuItem("TDFramework/一键打包/Build_GooglePlay/ABB")]
    public static void Build_GooglePlay2()
    {
        resetStyle();
        EditorUserBuildSettings.buildAppBundle = true;
        BuildAndroid();
    }

    [MenuItem("TDFramework/一键打包/Build_GooglePlay/mono")]
    public static void Build_GooglePlay1()
    {
        //PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
    }

    public static void BuildAndroid()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        SetPlayerSetting();
        VersionStyle style = VersionStyle.Instance;
        string gameName = style.JobName + "_" + style.FullVersion + "_" + style.Channel.ToString() + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm");
        if (style.IsGM)
            gameName += "_gm";
        //  var scenes = EditorBuildSettings.scenes;
        InitScenes();
        VersionStyle.Instance.Save();
        string[] levels = UnityEditor.EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        // string[] levels = { WashCar };
        string to = Path.GetFullPath(ApkPath);//+ style.Version + "/"
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);
        string save = to + gameName + ".apk";
        long t = System.DateTime.Now.Ticks;
        string aab = EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
        Debug.Log("开始打包：" + save + "," + aab);

        var res = BuildPipeline.BuildPlayer(levels, save, BuildTarget.Android, BuildOptions.None);

        VersionStyle.Instance.Save();
        long te = System.DateTime.Now.Ticks - t;
        if (EditorUserBuildSettings.buildAppBundle)
        {
            save = to + gameName + ".aab";
        }
        Debug.Log("耗时：" + (te * 0.0000001f).ToString("0.0") + " 秒，打包结束：" + save);

        string deleteUrl = Application.dataPath + "/../Library/il2cpp_android_armeabi-v7a/";
        if (Directory.Exists(deleteUrl))
        {
            string dir = Path.GetFullPath(deleteUrl);
            Directory.Delete(dir, true);
        }
        string deleteUrl2 = Application.dataPath + "/../Library/il2cpp_android_arm64-v8a/";
        if (Directory.Exists(deleteUrl2))
        {
            string dir = Path.GetFullPath(deleteUrl2);
            Directory.Delete(dir, true);
        }
        string deleteUrl3 = to + gameName + "-" + style.FullVersion + "-v" + style.build + ".symbols.zip";
        if (File.Exists(deleteUrl3))
        {
            string dir = Path.GetFullPath(deleteUrl3);
            File.Delete(dir);
        }
        string verDir = Application.dataPath + "/../../version.txt";
        if (!File.Exists(save))
        {
            File.Delete(verDir);
            Debug.LogError("build_failed:");
        }
        else
        {
            string fName = gameName + aab;
            string fUrl = "http://10.225.22.126:8080/job/" + style.JobName +  "/ws/" + fName;
            string desInfo = "<a href = '" + fUrl + "'>" + fName + "</a>";
            Debug.Log("Visit this URL to see: " + desInfo);

            FileStream fs = new FileStream(verDir, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string vs = "\rFileName:" + fName + "\r" + "FileUrl:" + fUrl;
            vs = "\rFileUrl:[" + fName + "](" + fUrl + ")";
            sw.Write(vs);
            sw.Close();
            fs.Close();
        }
    }


    public static void BuildWindows()
    {
        SetPlayerSetting();
        resetStyleWindows();
        VersionStyle style = VersionStyle.Instance;
        PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
        PlayerSettings.companyName = style.CompanyName;
        PlayerSettings.productName = style.JobName;
        PlayerSettings.resizableWindow = false;
        style.Save();
        InitScenes();
        string[] levels = UnityEditor.EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        string verStr = style.FullVersion;
        string to = Path.GetFullPath(ApkPath+ style.FullVersion+@"\");
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);
        
   

        string gameName = style.JobName + "_" + style.FullVersion + "_" + style.Channel.ToString() + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm");
        string save = to + gameName + ".exe";
        
        Debug.Log("开始打包：" + save);
        BuildPipeline.BuildPlayer(levels, save, BuildTarget.StandaloneWindows, BuildOptions.None);
        Debug.Log("打包结束：" + save);

    }

    static void resetStyle()
    {
        VersionStyle style = VersionStyle.Instance;
        style.IsGM = false;
        style.Channel = eChannel.Google;
        EditorUserBuildSettings.buildAppBundle = false;
    }


    static void resetStyleWindows()
    {
        VersionStyle style = VersionStyle.Instance;
        style.IsGM = false;
        style.Channel = eChannel.Windos;
    }

    static string keystore_FileName { get { return "yk"; } }
    static string keystorePass { get { return "chanxue"; } }
    static string keyaliasName { get { return "cx"; } }
    static string keyaliasPass { get { return "chanxue"; } }
 

    /*  创建秘钥
     keytool -exportcert -alias alias_kff -keystore D:\GitProject\CrowPixel\Client\Assets\CrowdPixel.keystore | openssl sha1 -binary | openssl base64
      输入密钥库口令:  crowdpixel
    F8ln5Xw3bFsHv3bx9MOR9Cs1QSo=
    */
    [MenuItem("TDFramework/一键打包/SetPlayerSetting")]
    private static void SetPlayerSetting()
    {
        VersionStyle style = VersionStyle.Instance;
        PlayerSettings.stripEngineCode = true;

        PlayerSettings.allowFullscreenSwitch = true;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;
        PlayerSettings.enableInternalProfiler = false;
        BuildTarget bTarget = EditorUserBuildSettings.activeBuildTarget;
        style.build++;
        style.iosBuild = 0;
        if (bTarget == BuildTarget.Android)
        {
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            //style._Language = eLanguage.auto;
            PlayerSettings.Android.bundleVersionCode = style.build;
            //  PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, style.IsGM ? ScriptingImplementation.Mono2x : ScriptingImplementation.IL2CPP);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low);
            //string keyName = "my";
            //string keyPath = EditorPath.SDKDir + "/Android/" + keyName + ".keystore";//my.keystore  "i4joy";
            //keyPath = Path.GetFullPath(keyPath);
            //if (style.Channel == eChannel.Google || style.Channel == eChannel.Onestore)
            //    keyPath = keyPath.Replace("\\", "/");
            PlayerSettings.Android.keystoreName = Application.dataPath + "/" + keystore_FileName + ".keystore";
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyaliasName; //keyName;// 
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
            PlayerSettings.Android.buildApkPerCpuArchitecture = false;
            PlayerSettings.Android.startInFullscreen = true;
        }
        else if (bTarget == BuildTarget.iOS)
        {
            //PlayerSettings.iOS.buildNumber = style.build.ToString();
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
        }
      
        PlayerSettings.bundleVersion = style.FullVersion;
        PlayerSettings.applicationIdentifier = style.bundleIdentifier;
        PlayerSettings.productName = style.productName;
    }

    [MenuItem("TDFramework/一键打包/IOS/Build_GooglePlay_IOS")]
    public static void Build_GooglePlay_IOS()
    {
        Build_IOS();
    }
    [MenuItem("TDFramework/一键打包/IOS/Build_GooglePlay_IOS_GM")]
    public static void Build_GooglePlay_IOS_GM()
    {
        VersionStyle.Instance.IsGM = true;
        Build_IOS();
    }

    public static string XCodeProjectPath()
    {
        VersionStyle style = VersionStyle.Instance;
        string bid = style.productName.Replace(" ", "") + "_" + style.version;
        string url = Application.dataPath + "/../../../build_xcode/" + bid;
        return Path.GetFullPath(url);
    }
    static void Build_IOS()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        SetPlayerSetting();
        InitScenes();
        VersionStyle.Instance.Save();
        string[] levels = UnityEditor.EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        // string[] levels = { WashCar };
        string save = XCodeProjectPath();
        if (!Directory.Exists(save))
            Directory.CreateDirectory(save);
        long t = System.DateTime.Now.Ticks;
        Debug.Log("开始打包：" + save);
        var res = BuildPipeline.BuildPlayer(levels, save, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);

        VersionStyle.Instance.Save();
        long te = System.DateTime.Now.Ticks - t;
        Debug.Log("耗时：" + (te * 0.0000001f).ToString("0.0") + " 秒，打包结束：" + save);
        if (res.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.LogError("build_failed:" + res.summary.result.ToString());
        }
        else
        {
            Debug.Log("build_succeed");
        }
    }

}
