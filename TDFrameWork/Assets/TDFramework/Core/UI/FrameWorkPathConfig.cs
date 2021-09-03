using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PathConfig", menuName = "TD/FrameWorkPathConfig")]
public class FrameWorkPathConfig : ScriptableObject
{
    public string m_UIScriptSavaPath = "/Scripts/UI/";
    public string m_UINameSpacePath = "Game.UI";
    public string m_ExcelDataPath = "Asset/Data";
    public string m_ExcelDataSavaPath = "Asset/Data";
    public string m_CShapScriptsSavaPath= "Asset/Scripts/Data";
}
