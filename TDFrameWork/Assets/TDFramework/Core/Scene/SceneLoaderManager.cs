using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace TDFramework
{
    public class SceneLoaderManager : ManagerBase, IDisposable
    {

        private LinkedList<SceneLoderRoutine> m_SceneLoaderList;

        private int m_CurLoadSceneId;

        private bool m_IsLoadingScene;

        private BaseAction m_LoadCompleteCallBack;

        private string m_CurSceneName;

        private Dictionary<int, float> m_SceneLoadProgressDic;

        private float m_CurSceneProgress;

        /// <summary>
		/// 需要加载或者卸载的明细数量
		/// </summary>
		private int m_NeedLoadOrUnloadSceneDetailCount = 0;

        /// <summary>
        /// 当前已经加载或者卸载的明细数量
        /// </summary>
        private int m_CurrLoadOrUnloadSceneDetailCount = 0;



        public void LoadScene(int sceneId,bool isOpenLoading,BaseAction complete=null)
        {
            if (m_IsLoadingScene)
            {
                GameEntry.Debug.Log("Scene Is Loading");
                return;
            }

            if (m_CurLoadSceneId == sceneId)
            {
                GameEntry.Debug.Log("Current SceneId == Scene ID ");
                m_LoadCompleteCallBack?.Invoke();
                return;
            }

            //StopBg ToDo
            m_LoadCompleteCallBack = complete;
          
            if (isOpenLoading)
            {

                //OpenUI() TODO 
                DoLoad(sceneId);
            }
            else
            {
                DoLoad(sceneId);
            }
        }

     
        private void DoLoad(int sceneId)
        {
            m_CurLoadSceneId = sceneId;
            m_IsLoadingScene = true;
            m_CurSceneProgress = 0;
            m_SceneLoadProgressDic.Clear();
            //卸载当前
            UnLoadScene();
        }

        private void UnLoadScene()
        {
            if (string.IsNullOrEmpty(m_CurSceneName))
            {
                LoadNewScene();
            }
            else
            {
                m_NeedLoadOrUnloadSceneDetailCount = 1;
                //卸载
                SceneLoderRoutine routine=GameEntry.Pool.PopClass<SceneLoderRoutine>();
                m_SceneLoaderList.AddLast(routine);
                routine.UnLoad(m_CurSceneName, UnLoadCompleteCallBack);
            }
        }

        private void UnLoadCompleteCallBack(SceneLoderRoutine loader)
        {
            if (loader == null)
                GameEntry.Logger.Write($"UnLoadCompleteCallBack Loader Is Null,Scene Name:{m_CurSceneName}",UnityEngine.LogType.Exception);

            m_SceneLoaderList.Remove(loader);
            GameEntry.Pool.PushClass<SceneLoderRoutine>(loader);

            LoadNewScene();
        }

        private void LoadNewScene()
        {
            m_NeedLoadOrUnloadSceneDetailCount = 1;
            //GetSceneName ToDo
            if(m_CurLoadSceneId==1)
                 m_CurSceneName = "Main"; 
            else
                m_CurSceneName = "Fight";

            SceneLoderRoutine routine= GameEntry.Pool.PopClass<SceneLoderRoutine>();
            m_SceneLoaderList.AddLast(routine);
            routine.LoadScene(m_CurLoadSceneId, m_CurSceneName, OnLoadSceneProgressUpdate, LoadCompleteCallBack);

        }

        private void LoadCompleteCallBack(SceneLoderRoutine routine)
        {
            if (routine == null)
                return;

            m_CurrLoadOrUnloadSceneDetailCount++;
            if (m_CurrLoadOrUnloadSceneDetailCount == m_NeedLoadOrUnloadSceneDetailCount)
            {
                m_SceneLoaderList.Remove(routine);
                GameEntry.Pool.PushClass<SceneLoderRoutine>(routine);
                m_CurrLoadOrUnloadSceneDetailCount = 0;
                m_NeedLoadOrUnloadSceneDetailCount = 0;
            }
          

        }

        private void OnLoadSceneProgressUpdate(float progress, int sceneId)
        {
            m_SceneLoadProgressDic[sceneId] = progress;
        }

     

        internal override void Init()
        {
            m_SceneLoaderList = new LinkedList<SceneLoderRoutine>();
            m_SceneLoadProgressDic = new Dictionary<int, float>();
        }



        public void Update()
        {
            if (m_IsLoadingScene)
            {
               var loader=m_SceneLoaderList.First;
                while (loader != null)
                {
                    loader.Value.Update();
                    loader = loader.Next;
                }

                float traget = GetProgress();
                float needProgress = 0.9f * m_NeedLoadOrUnloadSceneDetailCount;
                if (traget >= needProgress)
                    traget = needProgress;

                if(m_CurSceneProgress<= traget && m_CurSceneProgress < m_NeedLoadOrUnloadSceneDetailCount)
                {
                    m_CurSceneProgress = m_CurSceneProgress + UnityEngine.Time.deltaTime * m_NeedLoadOrUnloadSceneDetailCount*1;
                    float slider = m_CurSceneProgress / m_NeedLoadOrUnloadSceneDetailCount;
                    GameEntry.Event.Broadcast(1000, slider);

                }
                else if (m_CurSceneProgress>=m_NeedLoadOrUnloadSceneDetailCount)
                {
                                   
                    //Load Complete
                    Scene scene= SceneManager.GetSceneByName(m_CurSceneName);
                    if (!scene.isLoaded)
                        return;

                    


                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_CurSceneName));
     
                    m_IsLoadingScene = false;
                    m_NeedLoadOrUnloadSceneDetailCount = 0;
                    m_CurrLoadOrUnloadSceneDetailCount = 0;

                   
                    //OpenBg
                    //CloseLoadingUI
                    m_LoadCompleteCallBack?.Invoke();
                }
            }
        }


        private float GetProgress()
        {
            float progress = 0;
            var enumerator = m_SceneLoadProgressDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                progress += enumerator.Current.Value;
            }
            return progress;
        }

        public void Dispose()
        {

        }


    }
}
