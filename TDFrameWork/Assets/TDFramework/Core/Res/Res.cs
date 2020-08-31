using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDFramework.Resource
{
    public static class Res
    {

        #region 异步加载资源
        /// <summary>
        ///  异步加载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="_path">地址或者Label/param>
        /// <param name="_loaded">回调</param>
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string _path, Action<T> _loaded = null)
        {
           return ResManager.Instance.LoadAssetAsync<T>(_path, _loaded);
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
        /// <param name="trackHandle">当设置为false时，必须在释放实例时保留要使用的AsyncOperationHandle。这更有效率，但需要更多的开发精力。</param>
        public static AsyncOperationHandle<GameObject> InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
           return ResManager.Instance.InstanceAsync(_path, _loaded, parent, instantiateInWorldSpace, trackHandle);
        }
        #endregion

        #region  根据图集加载图片
        /// <summary>
        /// AsyncLoadSpriteByAltas
        /// </summary>
        /// <param name="path">地址或者Label</param>
        /// <param name="spriteName">图片名</param>
        /// <param name="_loaded">回调</param>
        public static void LoadAsyncSpriteByAltas(string _path, string spriteName, Action<Sprite> _loaded = null)
        {
            ResManager.Instance.LoadAsyncSpriteByAltas(_path, spriteName, _loaded);
        }
        #endregion

        #region 根据Label加载所有资源
        /// <summary>
        /// 根据Label加载所有资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="label">Label</param>
        /// <param name="_oneLoaded">加载完一个回调，但是并不知道是哪一个</param>
        /// <param name="completeCallBack">所有加载完回调</param>

        public static void LoadAssetsAsyncByLabel<T>(string label, Action<T> _oneLoaded = null, Action<IList<T>> completeCallBack = null)
        {
            ResManager.Instance.LoadAssetsAsyncByLabel<T>(label, _oneLoaded, completeCallBack);
        }
        #endregion

        #region 根据Label和Path进行组合标签加载
        /// <summary>
        /// 根据Label和Path进行组合标签加载
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="keys">Label和Path组成的list</param>
        /// <param name="mergeMode">MergeMode 1、默认选择第一个内容进行删选 2、并集 3、交集</param>
        /// <param name="_oneLoaded">加载完一个回调，但是并不知道是哪一个</param>
        /// <param name="completeCallBack">所有加载完回调</param>
        public static void LoadAssetsAsyncByLabelOrName<T>(IList<object> keys, Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection, Action<T> _oneLoaded = null, Action<IList<T>> completeCallBack = null)
        {
            ResManager.Instance.LoadAssetsAsyncByLabelOrName<T>(keys, mergeMode, _oneLoaded, completeCallBack);

        }
        #endregion





        #region 卸载资源
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">资源</param>
        public static void ReleaseAsset<T>(T source)
        {
            ResManager.Instance.ReleaseAsset<T>(source);
        }

        public static void ReleaseAsset<T>(AsyncOperationHandle<T> handle)
        {
             ResManager.Instance.ReleaseAsset<T>(handle);
        }

        public static bool ReleaseInstance(GameObject obj)
        {
            return ResManager.Instance.ReleaseInstance(obj);
        }

        public static bool ReleaseInstance(AsyncOperationHandle handle)
        {
            return ResManager.Instance.ReleaseInstance(handle);
        }

        public static bool ReleaseInstance(AsyncOperationHandle<GameObject> handle)
        {
            return ResManager.Instance.ReleaseInstance(handle);
        }

        #endregion
    }
}
