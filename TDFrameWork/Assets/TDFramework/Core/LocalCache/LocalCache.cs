using System.IO;
using UnityEngine;

namespace TDFramework.Cache
{
    [System.Serializable]
    public  class LocalCache
    {
        public LocalCache() { }

        public virtual void FirstInitCache() { }

        public virtual void ReadCache() { }

        public virtual void WriteCache() { }
       
    }

    public static class LocalCacheUtil
    {

        public static string CachePath = Application.persistentDataPath + "/Cache/";

        public static T Read<T>() where T : LocalCache, new()
        {

            string typeName = typeof(T).Name;
            T self = null;
            string typePath = $"{CachePath}{typeName}.txt";
            if (!File.Exists(typePath))
            {
                self = new T();
                self.FirstInitCache();
                if (!Directory.Exists(CachePath))
                    Directory.CreateDirectory(CachePath);
                File.Create(typePath);
            }
            else
            {
                string cache = TDFramework.Tool.StringEncryption.DecryptDES(File.ReadAllText(typePath));
                self = LitJson.JsonMapper.ToObject<T>(cache);
                GameEntry.Debug.Log($"CacheType[{typeName}]\n{cache}");
            }

            self.ReadCache();
            return self;
        }

        public static void Write<T>(T self) where T : LocalCache
        {
            if (self != null)
            {
                string typeName = typeof(T).Name;
                string typePath = $"{CachePath}{typeName}.txt";
                self.WriteCache();
                string cacheData = LitJson.JsonMapper.ToJson(self);
                GameEntry.Debug.Log($"CacheType[{typeName}]\n{cacheData}");
                string cache = TDFramework.Tool.StringEncryption.EncryptDES(cacheData);
                File.WriteAllText(typePath, cache);

            }
        }

       
    }
}
