using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework.Cache
{
    [System.Serializable]
    public abstract class LocalCache
    {
        public LocalCache() { }

        public abstract void FirstInitCache();


        public abstract void ReadCache();

        public abstract void WriteCache();
       
    }

    public static class LocalCacheUtil
    {
        public static T Read<T>()where T : LocalCache,new ()
        {

            string typeName = typeof(T).Name;
            T self = null;
            if (!UnityEngine.PlayerPrefs.HasKey(typeName))
            {
                self = new T();
                self.FirstInitCache();
            }
            else
            {
                string cache = TDFramework.Tool.StringEncryption.DecryptDES(UnityEngine.PlayerPrefs.GetString(typeName));
                self = LitJson.JsonMapper.ToObject<T>(cache);
            }

            self.ReadCache();
            return self;
        }

        public static void Write<T>(T self) where T : LocalCache
        {
            if (self != null)
            {
                self.WriteCache();
                string cache=TDFramework.Tool.StringEncryption.EncryptDES(LitJson.JsonMapper.ToJson(self));
                UnityEngine.PlayerPrefs.SetString(typeof(T).Name, cache);
            }
        }
    }
}
