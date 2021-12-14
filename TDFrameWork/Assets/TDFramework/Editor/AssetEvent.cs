using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AssetEvent : UnityEditor.AssetModificationProcessor
{

    #region ȫ�ּ���Project��ͼ�µ���Դ�Ƿ����仯����� ɾ�� �ƶ��ȣ�
    //    //[InitializeOnLoadMethod]
    //    //static void EditorApplication_projectChanged()
    //    //{
    //    //    //--projectWindowChanged�ѹ�ʱ
    //    //    //--ȫ�ּ���Project��ͼ�µ���Դ�Ƿ����仯����� ɾ�� �ƶ��ȣ�
    //    //    EditorApplication.projectChanged += delegate ()
    //    //    {
    //    //        Debug.Log("��Դ״̬�����仯��");
    //    //    };
    //    //}
    //    ////--������˫��������������Դ���¼�
    //    //public static bool IsOpenForEdit(string assetPath, out string message)
    //    //{
    //    //    message = null;
    //    //    Debug.Log("˫��������������Դ assetPath:" + assetPath);
    //    //    return true;
    //    //}
    #endregion

    #region --��������Դ�������������¼�
    public static void OnWillCreateAsset(string path)
    {

        var setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
        var group = setting.FindGroup("IL");
        if (group == null)
        {
            //�����ֶ����Group ��Ҳ�������·����д��� ���ǲ�����
            // setting.CreateGroup("IL", false, false, true, null, null);
            Debug.LogError("addressable not IL Group");
        }


        var assetDll = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Res/HotFix/dll_res.bytes");
        if (assetDll != null)
        {
            string str = AssetDatabase.GetAssetPath(assetDll);
            var guid = AssetDatabase.AssetPathToGUID(str); //���GUID
            var entry = setting.CreateOrMoveEntry(guid, group); //ͨ��GUID����entry
            entry.SetAddress("dll_res", true);
            entry.SetLabel("IL", true);
        }



        var assetPdb = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Res/HotFix/pdb_res.bytes");
        if (assetPdb != null)
        {
            string str = AssetDatabase.GetAssetPath(assetPdb);
            var guid = AssetDatabase.AssetPathToGUID(str); //���GUID
            var entry = setting.CreateOrMoveEntry(guid, group); //ͨ��GUID����entry
            entry.SetAddress("pdb_res", true);
            entry.SetLabel("IL", true);

        }

 
        AssetDatabase.Refresh();


    }
    #endregion

    #region --��������Դ���������桱�¼�
    //        //public static string[] OnWillSaveAssets(string[] paths)
    //        //{
    //        //    if (paths != null)
    //        //    {
    //        //        Debug.Log("��Դ���������� path :" + string.Join(",", paths));
    //        //    }
    //        //    return paths;
    //        //}
    //        ////--��������Դ�������ƶ����¼�
    //        //public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    //        //{
    //        //    Debug.Log("��Դ�������ƶ� form:" + oldPath + " to:" + newPath);
    //        //    return AssetMoveResult.DidNotMove;
    //        //}
    //        ////--��������Դ������ɾ�����¼�
    //        //public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    //        //{
    //        //    Debug.Log("��Դ������ɾ�� : " + assetPath);
    //        //    return AssetDeleteResult.DidNotDelete;
    //        //}
    #endregion
}
