
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework.Localization
{
    public class LocalizationMgr:ManagerBase,IDisposable
    {
        public enum LocalizationType
        {
            None,
            CH,
            EN,
            JP,
        }

        private const string playerprefsName = "LocalizationType";

        private Dictionary<short, string> m_TextLocalDic=new Dictionary<short, string>();
        private Dictionary<short, string> m_PicLocalDic=new Dictionary<short, string>();
        private LocalizationType m_CurLocalizationType;
        private Action m_RefreshHandle;
      
        public Action RefreshHandle { get => m_RefreshHandle; set => m_RefreshHandle = value; }

  

        public void ChangeLanguageType(LocalizationType _type)
        {
            if (m_CurLocalizationType == _type)
                return;

            m_TextLocalDic.Clear();
            switch (_type)
            {
                //TODO load data
                case LocalizationType.CH:
                    m_TextLocalDic.Add(10000, "测试一");
                    break;
                case LocalizationType.EN:
                    m_TextLocalDic.Add(10000, "test one");
                    break;
                case LocalizationType.JP:
                    m_TextLocalDic.Add(10000, "ぬひむもちちの");
                    break;
                default:
                    break;
            }

            PlayerPrefs.SetInt(playerprefsName, m_CurLocalizationType.GetHashCode());
            RefreshHandle?.Invoke();
        }


  
        public string GetTextByKey(short key)
        {
            if (m_TextLocalDic == null)
                Init();
            if (!m_TextLocalDic.ContainsKey(key))
                return "dic not has this key";

            return m_TextLocalDic[key];
        }

        public UnityEngine.Sprite GetSpriteByKey(short key)
        {
            if (m_PicLocalDic == null)
                Init();
            if (!m_PicLocalDic.ContainsKey(key))
                throw new Exception("dic not has this key");

            return null;
            //return picLocalDic[key];
        }

        internal override void Init()
        {
            LocalizationType _type = LocalizationType.CH;
            if (!PlayerPrefs.HasKey(playerprefsName) || (LocalizationType)PlayerPrefs.GetInt(playerprefsName) == LocalizationType.None)
                _type = LocalizationType.EN;
            else
                _type = (LocalizationType)PlayerPrefs.GetInt(playerprefsName);

            ChangeLanguageType(_type);
        }

        public void Dispose()
        {
            m_TextLocalDic.Clear();
            m_PicLocalDic.Clear();
            m_RefreshHandle = null;
        }
    }
}
