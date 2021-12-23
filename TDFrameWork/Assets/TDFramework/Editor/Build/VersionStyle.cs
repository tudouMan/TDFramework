using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace TDFramework
{
    [CreateAssetMenu(menuName = "Creat VersionStyle",fileName = "VersionStyle")]
    public class VersionStyle:ScriptableObject
    {
        public string FullVersion { get { return this.version + "." + build; } }
        public string version { get { return "0.2"; } }
        public int build = 100;
        public int iosBuild = 0;
        public string productName { get { return "test"; } }
        public string JobName = "Test";
        public string bundleIdentifier
        {
            get
            {
                if (Channel == eChannel.Apple) return "com.text.project";
                return "com.text.project";
            }
        }


        public string CompanyName { get=>"TUDOU"; }

        public bool isLoadAB = false;
        public bool isTestLoadAB
        {
            get
            {
#if !UNITY_EDITOR
            return true;
#endif
                return isLoadAB;
            }
        }
        public eChannel Channel = eChannel.Google;
        public bool IsGM = false;
        private static VersionStyle _Instance = null;
        public static VersionStyle Instance
        {
            get
            {
#if UNITY_EDITOR
                string url = @"Assets/TDFramework/Resources/VersionStyle.asset";
                if (!File.Exists(url))
                {
                    VersionStyle newAsset = ScriptableObject.CreateInstance<VersionStyle>();
                    UnityEditor.AssetDatabase.CreateAsset(newAsset, url);
                    UnityEditor.AssetDatabase.ImportAsset(url);
                }
                _Instance = UnityEditor.AssetDatabase.LoadAssetAtPath(url, typeof(VersionStyle)) as VersionStyle;

#endif
                if (_Instance == null)
                    _Instance = Resources.Load("VersionStyle") as VersionStyle;
                return _Instance;
            }
        }


        public void Save()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(VersionStyle.Instance);
#endif
        }

    }
}
