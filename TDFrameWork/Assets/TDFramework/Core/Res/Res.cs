using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework.Resource
{
    public interface  Res
    {

        /// <summary>
        ///  异步加载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="_path">地址或者Label/param>
        /// <param name="_loaded">回调</param>
        public void LoadAssetAsync<T>(string _path, Action<T> _loaded = null)where T:UnityEngine.Object;

        /// <summary>
        /// Async Instance
        /// </summary>
        /// <param name="_path">地址或者Label</param>
        /// <param name="_loaded">回调</param>
        /// <param name="parent">Parent</param>
        /// <param name="是否生成在世界空间">instantiateInWorldSpace</param>
        /// <param name="trackHandle">跟踪节点</param>
        public void InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true);


        /// <summary>
        /// AsyncLoadSpriteByAltas
        /// </summary>
        /// <param name="path">地址或者Label</param>
        /// <param name="spriteName">图片名</param>
        /// <param name="_loaded">回调</param>
        public void LoadAsyncSpriteByAltas(string path, string spriteName, Action<Sprite> _loaded = null);


        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">资源</param>
        public void ReleaseAsset<T>(T source);

       

        public void ReleaseInstance(GameObject source);

        public T LoadAsset<T>(string path) where T : UnityEngine.Object;

        public GameObject Instance(string path, Transform root);
        public void LoadAssetsAsyncByLabel<T>(string label, Action<IList<T>> completeCallBack = null);


    }
}
