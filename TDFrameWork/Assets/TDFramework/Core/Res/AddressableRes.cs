using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine;

namespace TDFramework.Resource
{
    public class AddressableRes : Res
    {
        #region 异步加载资源
        /// <summary>
        ///  异步加载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="_path">地址或者Label/param>
        /// <param name="_loaded">回调</param>
        public  void LoadAssetAsync<T>(string _path, Action<T> _loaded = null)where T:UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(_path);
            handle.Completed += (p) =>
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    _loaded?.Invoke(p.Result);
                else
                    throw new Exception("load fail: " + _path);
            };
        }
        #endregion

        #region 异步实例化

        /// <summary>
        /// Async Instance
        /// </summary>
        /// <param name="_path">地址或者Label</param>
        /// <param name="_loaded">回调</param>
        /// <param name="parent">Parent</param>
        /// <param name="是否生成在世界空间">instantiateInWorldSpace</param>
        /// <param name="trackHandle">跟踪节点</param>
        public void InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
            var handle = Addressables.InstantiateAsync(_path, parent, instantiateInWorldSpace, trackHandle);
            handle.Completed += (p) =>
            {
                _loaded?.Invoke(p.Result);
            };
        }

        #endregion

        #region  根据图集加载图片
        /// <summary>
        /// AsyncLoadSpriteByAltas
        /// </summary>
        /// <param name="path">地址或者Label</param>
        /// <param name="spriteName">图片名</param>
        /// <param name="_loaded">回调</param>
        public void LoadAsyncSpriteByAltas(string path, string spriteName, Action<Sprite> _loaded = null)
        {
            var op = Addressables.LoadAssetAsync<SpriteAtlas>(path);
            op.Completed += (p) =>
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    _loaded?.Invoke(p.Result.GetSprite(spriteName));
                else
                    throw new Exception("load fail: " + path);
            };

            Addressables.Release(op);
        }
        #endregion

        #region 根据Label加载所有资源
        /// <summary>
        /// 根据Label加载所有资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="label">Label</param>
        /// <param name="completeCallBack">所有加载完回调</param>
        public void LoadAssetsAsyncByLabel<T>(string label, Action<IList<T>> completeCallBack = null)
        {
            var op = Addressables.LoadAssetsAsync<T>(label, null);
            op.Completed += p =>
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    completeCallBack?.Invoke(p.Result);
                else
                    throw new Exception("load label: " + label);
            };

            Addressables.Release(op);
        }
        #endregion



        #region 卸载资源
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">资源</param>
        public void ReleaseAsset<T>(T source)
        {
            Addressables.Release<T>(source);

        }

        public void ReleaseAsset<T>(AsyncOperationHandle<T> handle)
        {
            Addressables.Release<T>(handle);

        }

        public void ReleaseInstance(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }

        public bool ReleaseInstance(AsyncOperationHandle handle)
        {
            return Addressables.ReleaseInstance(handle);
        }

        public bool ReleaseInstance(AsyncOperationHandle<GameObject> handle)
        {
            return Addressables.ReleaseInstance(handle);
        }


        #endregion

        #region 同步

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            var op = Addressables.LoadAssetAsync<T>(path);
            T go = op.WaitForCompletion();
            Addressables.Release(op);
            return go;
        }


        public GameObject Instance(string path, Transform root)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(path);
            GameObject go = op.WaitForCompletion();
            if (root != null)
                go.transform.parent = root;
            Addressables.Release(op);
            return go;
        }

     

        #endregion
    }
}


