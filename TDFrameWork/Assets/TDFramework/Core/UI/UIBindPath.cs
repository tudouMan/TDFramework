using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIBindPath",menuName ="TD/UI/CreatUIBindPath")]
public class UIBindPath : ScriptableObject
{
    public string m_UIInitPath = "/Scripts/UI/";
    public string m_UINameSpacePath = "Game.UI";
    public string m_ExcelDataPath = "";
    public string m_ExcelEncrypt = "";
    public string m_ExcelDataSavaPath = "";
}
