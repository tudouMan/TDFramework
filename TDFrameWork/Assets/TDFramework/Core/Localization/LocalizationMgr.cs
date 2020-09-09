
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework.Localization
{
    public class LocalizationMgr:Singleton<LocalizationMgr>
    {
        public enum LocalizationType
        {
            None,
            CH,
            EN,
            JP,
        }

        private const string playerprefsName = "LocalizationType";

        private Dictionary<short, string> mTextLocalDic=new Dictionary<short, string>();
        private Dictionary<short, string> mPicLocalDic=new Dictionary<short, string>();
        private LocalizationType mCurLocalizationType;
        private Action mRefreshHandle;
      
        public Action RefreshHandle { get => mRefreshHandle; set => mRefreshHandle = value; }

        private LocalizationMgr()
        {
          
        }

        public void ChangeLanguageType(LocalizationType _type)
        {
            if (mCurLocalizationType == _type)
                return;

            mTextLocalDic.Clear();
            switch (_type)
            {
                //TODO load data
                case LocalizationType.CH:
                    mTextLocalDic.Add(10000, "测试一");
                    break;
                case LocalizationType.EN:
                    mTextLocalDic.Add(10000, "test one");
                    break;
                case LocalizationType.JP:
                    mTextLocalDic.Add(10000, "ぬひむもちちの");
                    break;
                default:
                    break;
            }

            PlayerPrefs.SetInt(playerprefsName, mCurLocalizationType.GetHashCode());

            RefreshHandle?.Invoke();
        }


        public void Init()
        {
          
            LocalizationType _type = LocalizationType.CH;
            if (!PlayerPrefs.HasKey(playerprefsName) || (LocalizationType)PlayerPrefs.GetInt(playerprefsName)==LocalizationType.None)
                _type = LocalizationType.EN;
            else
                _type = (LocalizationType)PlayerPrefs.GetInt(playerprefsName);

            ChangeLanguageType(_type);
        }

        public string GetTextByKey(short key)
        {
            if (mTextLocalDic == null)
                Init();
            if (!mTextLocalDic.ContainsKey(key))
                return "dic not has this key";

            return mTextLocalDic[key];
        }

        public UnityEngine.Sprite GetSpriteByKey(short key)
        {
            if (mPicLocalDic == null)
                Init();
            if (!mPicLocalDic.ContainsKey(key))
                throw new Exception("dic not has this key");

            return null;
            //return picLocalDic[key];
        }

    }
}
