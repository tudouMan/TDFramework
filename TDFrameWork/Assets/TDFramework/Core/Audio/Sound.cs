using System;
using UnityEngine;
using TDFramework.Resource;
using DG.Tweening;
using TDFramework.Pool;

namespace TDFramework.Audio
{
 
    public class Sound
    {
        private AudioClip mCurAudioClip;
        private AudioSource mCurAudioSource;
        private bool mIsLoop;


        public bool IsPlaying
        {
            get
            {
                return mCurAudioSource.isPlaying;
            }
        }

        public float Progress
        {
            get
            {
                return (float)mCurAudioSource.timeSamples / (float)mCurAudioClip.samples;
            }
        }

        public bool IsFinish
        {
            get
            {
                return !mIsLoop && Progress >= 1;
            }
        }

        public bool IsLoop { get => mIsLoop; set => mIsLoop = value; }

        public void Play(AudioClip clip,bool isLoop,float vol,AudioSource source=null)
        {
            mIsLoop = isLoop;
            mCurAudioClip = clip;
            if (source != null)
                mCurAudioSource = source;

            if (mCurAudioSource != null)
            {
                mCurAudioSource.clip = clip;
                mCurAudioSource.loop = isLoop;
                mCurAudioSource.volume = vol;
                mCurAudioSource.Play();
            }
            else
            {
                Debug.Log("audio source is null");
            }
            
        }

        public void Pause()
        {
            if (mCurAudioSource != null)
            {
                mCurAudioSource.Pause();
            }
               
        }

        public void ContinuePlay()
        {
            if (mCurAudioSource != null)
                mCurAudioSource.Play();
        }

        public void Stop()
        {
            if (mCurAudioSource != null)
            {
                mCurAudioSource.Stop();
                TDFramework.GameEntry.Pool.PushClass<Sound>(this);
                GameEntry.Sound.Remove(this);
            }
               
        }

       
        public void Pop(object[] parmas = null)
        {
          
        }

        public void Push()
        {

            GameEntry.Res.ReleaseAsset<AudioClip>(mCurAudioClip);
        }

        public void FadeStop()
        {
           DG.Tweening.DOTween.To(() => mCurAudioSource.volume, x => mCurAudioSource.volume = x, 0, 0.3f).OnComplete(()=> 
           {
               Stop();
           });
        }

        public void OnInit(object[] parmas = null)
        {
           
        }
    }
}
