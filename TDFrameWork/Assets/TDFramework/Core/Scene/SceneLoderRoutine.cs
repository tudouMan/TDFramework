using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace TDFramework
{

    public class SceneLoderRoutine
    {

        private int m_SceneId;

        private AsyncOperation m_CurAsync;

        private BaseAction<float, int> m_UpdatePregressAction;

        private BaseAction<SceneLoderRoutine> m_LoadCompleteAction;

        private BaseAction<SceneLoderRoutine> m_UnLoadAction;

        public void LoadScene(int sceneId, string sceneName, BaseAction<float, int> updateProgress, BaseAction<SceneLoderRoutine> loadComplete)
        {
            Reset();
            m_SceneId = sceneId;
            m_UpdatePregressAction = updateProgress;
            m_LoadCompleteAction = loadComplete;
            //根据资源加载类型来做不同加载
#if UNITY_EDITOR || Resource
            m_CurAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            m_CurAsync.allowSceneActivation = false;
            if (m_CurAsync == null)
            {
                m_LoadCompleteAction?.Invoke(this);
                return;
            }

#else
           m_CurAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                m_CurAsync.allowSceneActivation = false;
                if (m_CurAsync == null)
                {
                    m_LoadCompleteAction?.Invoke(this);
                    return;
                }

#endif
          

        }

        public void UnLoad(string sceneName,BaseAction<SceneLoderRoutine>unLoadComplete)
        {
            Reset();
            m_UnLoadAction = unLoadComplete;
            m_CurAsync = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            if (m_CurAsync == null)
                m_UnLoadAction?.Invoke(this);
            GameEntry.Debug.Log("UnLoad Scene Complete: " + sceneName,DebugColoyType.Green);
        }


        private void Reset()
        {
            m_CurAsync = null;
            m_UpdatePregressAction = null;
            m_LoadCompleteAction=null;
            m_UnLoadAction = null;
        }

        public void Update()
        {
            if (m_CurAsync == null)
                return;

            if (!m_CurAsync.isDone)
            {
                if (m_CurAsync.progress >=0.9f)
                {
                    m_CurAsync.allowSceneActivation = true;
                    m_UpdatePregressAction?.Invoke(m_CurAsync.progress, m_SceneId);
                    m_CurAsync = null;
                    m_LoadCompleteAction?.Invoke(this);
                }
                else
                {
                    m_UpdatePregressAction?.Invoke(m_CurAsync.progress, m_SceneId);
                }
            }
            else
            {
                m_CurAsync = null;
                m_UnLoadAction?.Invoke(this);
            }
        }
    }
}
