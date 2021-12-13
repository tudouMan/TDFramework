using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AssetEvent : UnityEditor.AssetModificationProcessor
{

    #region 全局监听Project视图下的资源是否发生变化（添加 删除 移动等）
    //    //[InitializeOnLoadMethod]
    //    //static void EditorApplication_projectChanged()
    //    //{
    //    //    //--projectWindowChanged已过时
    //    //    //--全局监听Project视图下的资源是否发生变化（添加 删除 移动等）
    //    //    EditorApplication.projectChanged += delegate ()
    //    //    {
    //    //        Debug.Log("资源状态发生变化！");
    //    //    };
    //    //}
    //    ////--监听“双击鼠标左键，打开资源”事件
    //    //public static bool IsOpenForEdit(string assetPath, out string message)
    //    //{
    //    //    message = null;
    //    //    Debug.Log("双击鼠标左键，打开资源 assetPath:" + assetPath);
    //    //    return true;
    //    //}
    #endregion

    #region --监听“资源即将被创建”事件
    public static void OnWillCreateAsset(string path)
    {

        var setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
        var group = setting.FindGroup("IL");
        if (group == null)
        {
            //建议手动添加Group ，也可以用下方进行创建 但是不建议
            // setting.CreateGroup("IL", false, false, true, null, null);
            Debug.LogError("addressable not IL Group");
        }


        var assetDll = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Res/HotFix/dll_res.bytes");
        if (assetDll != null)
        {
            string str = AssetDatabase.GetAssetPath(assetDll);
            var guid = AssetDatabase.AssetPathToGUID(str); //获得GUID
            var entry = setting.CreateOrMoveEntry(guid, group); //通过GUID创建entry
            entry.SetAddress("dll_res", true);
            entry.SetLabel("IL", true);
        }



        var assetPdb = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Res/HotFix/pdb_res.bytes");
        if (assetPdb != null)
        {
            Debug.Log("creat path:" + path);
            string str = AssetDatabase.GetAssetPath(assetPdb);
            var guid = AssetDatabase.AssetPathToGUID(str); //获得GUID
            var entry = setting.CreateOrMoveEntry(guid, group); //通过GUID创建entry
            entry.SetAddress("pdb_res", true);
            entry.SetLabel("IL", true);

        }

 
        AssetDatabase.Refresh();


    }
    #endregion

    #region --监听“资源即将被保存”事件
    //        //public static string[] OnWillSaveAssets(string[] paths)
    //        //{
    //        //    if (paths != null)
    //        //    {
    //        //        Debug.Log("资源即将被保存 path :" + string.Join(",", paths));
    //        //    }
    //        //    return paths;
    //        //}
    //        ////--监听“资源即将被移动”事件
    //        //public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    //        //{
    //        //    Debug.Log("资源即将被移动 form:" + oldPath + " to:" + newPath);
    //        //    return AssetMoveResult.DidNotMove;
    //        //}
    //        ////--监听“资源即将被删除”事件
    //        //public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    //        //{
    //        //    Debug.Log("资源即将被删除 : " + assetPath);
    //        //    return AssetDeleteResult.DidNotDelete;
    //        //}
    #endregion
}
