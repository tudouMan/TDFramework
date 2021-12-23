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


        public void PlaySound(string guid, bool isLoop, float vol, EVENT_CALLBACK callBack = null)
        {
            PlayAudio(guid, false, vol);
        }


        public void PlayMusic(string soundName, float vol)
        {
            PlayAudio(soundName, true, vol);
        }

        private void PlayAudio(string guid, bool isLoop, float vol, EVENT_CALLBACK callBack = null)
        {
            GameEntry.Res.LoadAssetAsync<TextAsset>(guid, p =>
                {
                    FMODUnity.RuntimeManager.LoadBank(p);
                    EventInstance clipInfo = FMODUnity.RuntimeManager.CreateInstance(guid);
                    clipInfo.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Vector3.zero));
                    clipInfo.setVolume(vol);
                    clipInfo.setCallback(callBack);
                    clipInfo.start();
                });
        }


        public void Remove(string sound)
        {
            if (m_SoundsDic.ContainsKey(sound))
            {
                EventInstance clip=   m_SoundsDic[sound];
                clip.stop(STOP_MODE.IMMEDIATE);
                m_SoundsDic.Remove(sound);
                FMODUnity.RuntimeManager.UnloadBank(sound);
            } 
        }

       

        internal override void Init()
        {
            m_SoundsDic = new Dictionary<string, EventInstance>();
        }

        public void Dispose()
        {

        }

 

    }
}
