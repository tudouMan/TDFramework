using System;
using System.Collections.Generic;
using UnityEngine;


namespace TDFramework.Resource
{

    public class ResManager : ManagerBase, IDisposable
    {
        private Res m_Loader;

        public void Dispose()
        {
            m_Loader = null;
        }

        public GameObject Instance(string path, Transform root)
        {
            return m_Loader.Instance(path, root);
        }

        public void InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
             m_Loader.InstanceAsync(_path, _loaded, parent, instantiateInWorldSpace, trackHandle);
        }

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return m_Loader.LoadAsset<T>(path);
        }

        public void LoadAssetAsync<T>(string _path, Action<T> _loaded = null) where T : UnityEngine.Object
        {
            m_Loader.LoadAssetAsync<T>(_path, _loaded);
        }

        public void LoadAssetsAsyncByLabel<T>(string label, Action<IList<T>> completeCallBack = null)
        {
            m_Loader.LoadAssetsAsyncByLabel<T>(label, completeCallBack);
        }

        public void LoadAsyncSpriteByAltas(string path, string spriteName, Action<Sprite> _loaded = null)
        {
            m_Loader.LoadAsyncSpriteByAltas(path, spriteName, _loaded);
        }

        public void ReleaseAsset<T>(T source)
        {
            m_Loader.ReleaseAsset<T>(source);
        }

        public void ReleaseInstance(GameObject source)
        {
            m_Loader.ReleaseInstance(source);
        }

        internal override void Init()
        {
            if (GameEntry.Config.m_LoadType == LoadType.Addressable)
                m_Loader = new AddressableRes();
            else if (GameEntry.Config.m_LoadType == LoadType.Resouces)
                m_Loader = new ResourcesRes();
            else
                m_Loader = new ResourcesRes();

            GameEntry.Debug.Log($"GameResType Is [{GameEntry.Config.m_LoadType.ToString()}]");
        }
    }
}
