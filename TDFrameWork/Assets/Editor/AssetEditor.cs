using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetEditor :Editor
{
    [MenuItem("Tools/BuildAsset")]
    public static void Build()
    {
      
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Bundle/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }


    static string NormalPath=Application.dataPath+ "/Assets/DLL/";
    [MenuItem("MyMenu/ILRuntime/DLL To byte[]")]
    public static void DLLToBytes()
    {
        DLLToBytes(true);
    }
    [MenuItem("MyMenu/ILRuntime/DLL To byte[] (Choose Folder)")]
    public static void DLLToBytes_Choose()
    {
        DLLToBytes(false);
    }

    private static void DLLToBytes(bool autoChoosePath)
    {
        string folderPath;
        if (autoChoosePath)
            folderPath = NormalPath;
        else
            folderPath = EditorUtility.OpenFolderPanel("选择 DLL 所在的文件夹", Application.dataPath + "/Addressable/ILRuntime", string.Empty);
        if (string.IsNullOrEmpty(folderPath)) return;

        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        if (directoryInfo == null) return;

        FileInfo[] fileInfos = directoryInfo.GetFiles();

        List<FileInfo> listDLL = new List<FileInfo>();
        List<FileInfo> listPDB = new List<FileInfo>();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Extension == ".dll")
            {
                listDLL.Add(fileInfos[i]);
            }
            else if (fileInfos[i].Extension == ".pdb")
            {
                listPDB.Add(fileInfos[i]);
            }
        }

        if (listDLL.Count + listPDB.Count <= 0)
        {
            Debug.Log("文件夹下没有dll文件");
            return;
        }
        else
        {
            Debug.Log("选择路径为:" + folderPath);
        }

        string savePath;
        if (autoChoosePath)
            savePath = NormalPath;
        else
            savePath = EditorUtility.OpenFolderPanel("选择 DLL 转换后保存的文件夹", Application.dataPath + "/Addressable/ILRuntime", string.Empty);
        if (string.IsNullOrEmpty(savePath)) return;

        Debug.Log("---开始转换 DLL 文件------------------");
        string path = string.Empty;
        for (int i = 0; i < listDLL.Count; i++)
        {
            path = $"{savePath}/{Path.GetFileNameWithoutExtension(listDLL[i].Name)}_dll_res.bytes";
            BytesToFile(path, FileToBytes(listDLL[i]));
        }
        Debug.Log("---DLL 文件转换结束------------------");

        Debug.Log("---开始转换 PDB 文件------------------");
        for (int i = 0; i < listPDB.Count; i++)
        {
            path = $"{savePath}/{Path.GetFileNameWithoutExtension(listPDB[i].Name)}_pdb_res.bytes";
            BytesToFile(path, FileToBytes(listPDB[i]));
        }
        Debug.Log("---PDB 文件转换结束------------------");
        Debug.Log("导出路径为:" + savePath);

        AssetDatabase.Refresh();
    }

    private static byte[] FileToBytes(FileInfo fileInfo)
    {
        return File.ReadAllBytes(fileInfo.FullName);
    }
    private static void BytesToFile(string path, byte[] bytes)
    {
        Debug.Log($"Path:{path}\nlength:{bytes.Length}");
        File.WriteAllBytes(path, bytes);
    }

}
