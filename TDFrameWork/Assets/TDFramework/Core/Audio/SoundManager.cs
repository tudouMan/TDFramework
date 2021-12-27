using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


//*******使用FMOD第三方进行播放播放Sound*********
//master bank.strings.bank和master bank.bank必须加载，作为stream模式载入的资源会自动加载此文件，但是作为assetbundle时，需要自己加载，切记
namespace TDFramework.Audio
{
    public class SoundManager : ManagerBase, IDisposable
    {
        private Dictionary<string, EventInstance> m_SoundsDic;

        internal void LoadBank(Action onComplete=null)
        {
            GameEntry.Res.LoadAssetsAsyncByLabel<TextAsset>("FMOD", results => 
            {
                foreach (var item in results)
                {
                    FMODUnity.RuntimeManager.LoadBank(item);
                }

                onComplete?.Invoke();
            });
        }



        public void PlaySound(string guid, bool isLoop, float vol, EVENT_CALLBACK onComplete = null)
        {
            PlayAudio(guid, false, vol);
        }


        public void PlayMusic(string soundName, float vol)
        {
            PlayAudio(soundName, true, vol);
        }

        private void PlayAudio(string guid, bool isLoop, float vol, EVENT_CALLBACK onComplete = null)
        {
            EventInstance clipInfo;
            if (m_SoundsDic.ContainsKey(guid))
            {
                clipInfo = m_SoundsDic[guid];
            }
            else
            {
                clipInfo = FMODUnity.RuntimeManager.CreateInstance(guid);
                clipInfo.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Vector3.zero));
                m_SoundsDic.Add(guid, clipInfo);
            }

            clipInfo.setVolume(vol);
            clipInfo.setCallback(onComplete);
            clipInfo.start();

        }


        public void Remove(string sound)
        {
            if (m_SoundsDic.ContainsKey(sound))
            {
                EventInstance clip=   m_SoundsDic[sound];
                clip.stop(STOP_MODE.IMMEDIATE);
                clip.release();
                m_SoundsDic.Remove(sound);
            } 
        }

        public void RemoveAll()
        {
           var enumerator= m_SoundsDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                EventInstance eventInstance = enumerator.Current.Value;
                if (eventInstance.isValid())
                {
                    var reselt = eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    eventInstance.release();
                }
            }
           
            m_SoundsDic.Clear();

        }

       

        internal override void Init()
        {
            m_SoundsDic = new Dictionary<string, EventInstance>();
          //  LoadBank();
        }




        public void Dispose()
        {
         
        }

 

    }
}
