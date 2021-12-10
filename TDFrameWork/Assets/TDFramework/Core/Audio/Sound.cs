using System;
using UnityEngine;
using TDFramework.Resource;
using TDFramework.Pool;

namespace TDFramework.Audio
{
 
    public class Sound
    {
        private AudioClip m_CurAudioClip;
        private AudioSource m_CurAudioSource;
        private bool m_IsLoop;


        public bool IsPlaying
        {
            get
            {
                return m_CurAudioSource.isPlaying;
            }
        }

        public float Progress
        {
            get
            {
                return (float)m_CurAudioSource.timeSamples / (float)m_CurAudioClip.samples;
            }
        }

        public bool IsFinish
        {
            get
            {
                return !m_IsLoop && Progress >= 1;
            }
        }

        public bool IsLoop { get => m_IsLoop; set => m_IsLoop = value; }

        public void Play(AudioClip clip,bool isLoop,float vol,AudioSource source=null)
        {
            m_IsLoop = isLoop;
            m_CurAudioClip = clip;
            if (source != null)
                m_CurAudioSource = source;
            
            if (m_CurAudioSource != null)
            {
                m_CurAudioSource.clip = clip;
                m_CurAudioSource.loop = isLoop;
                m_CurAudioSource.volume = vol;
                m_CurAudioSource.Play();
            }
            else
            {
                Debug.Log("audio source is null");
            }
            
        }

        public void Pause()
        {
            if (m_CurAudioSource != null)
            {
                m_CurAudioSource.Pause();
            }
               
        }

        public void ContinuePlay()
        {
            if (m_CurAudioSource != null)
                m_CurAudioSource.Play();
        }

        public void Stop()
        {
            if (m_CurAudioSource != null)
            {
                m_CurAudioSource.Stop();
                TDFramework.GameEntry.Pool.PushClass<Sound>(this);
                GameEntry.Sound.Remove(this);
            }
               
        }

       
        public void Pop(object[] parmas = null)
        {
          
        }

        public void Push()
        {
            GameEntry.Res.ReleaseAsset<AudioClip>(m_CurAudioClip);
        }

        public void FadeStop()
        {
           //DG.Tweening.DOTween.To(() => m_CurAudioSource.volume, x => m_CurAudioSource.volume = x, 0, 0.3f).OnComplete(()=> 
           //{
           //    Stop();
           //});
        }

        public void OnInit(object[] parmas = null)
        {
           
        }
    }
}
