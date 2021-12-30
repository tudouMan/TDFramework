using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace TDFramework.Resource
{
    public class ResourcesRes : Res
    {
        public GameObject Instance(string path, Transform root)
        {
           return GameObject.Instantiate(LoadAsset<GameObject>(path), root);
        }

        public void InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(_path);
            request.completed += p =>
            {
                GameObject result=  GameObject.Instantiate(request.asset as GameObject, parent,instantiateInWorldSpace);
                _loaded?.Invoke(result);
            };

        }

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        public void LoadAssetAsync<T>(string _path, Action<T> _loaded = null) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(_path);
            request.completed += p => 
            {
                if (request.asset != null)
                    _loaded?.Invoke(request.asset as T);
            };
        }

        public void LoadAssetsAsyncByLabel<T>(string label, Action<IList<T>> completeCallBack = null)
        {
            
        }

        public void LoadAsyncSpriteByAltas(string path, string spriteName, Action<Sprite> _loaded = null)
        {
            LoadAssetAsync<SpriteAtlas>(path, p =>
            {
                _loaded?.Invoke(p.GetSprite(spriteName));
            });
        }

        public void ReleaseAsset<T>(T source)
        {
            Resources.UnloadAsset(source as UnityEngine.Object);
        }

        public void ReleaseInstance(GameObject source)
        {
            Resources.UnloadAsset(source);
        }
    }
}
