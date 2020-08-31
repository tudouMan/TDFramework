using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TDFramework.HeapPool;
using TDFramework.Resource;
using DG.Tweening;

namespace TDFramework.Audio
{
 
    public class Sound:IHeapObject
    {
        private AudioClip mCurAudioClip;
        private AudioSource mCurAudioSource;
        private bool mIsLoop;
        private HeapScriptsPool<Sound> mSoundHeap;

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

        private void Play(AudioClip clip,bool isLoop,float vol)
        {
            mIsLoop = isLoop;
            mCurAudioClip = clip;
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
                mCurAudioSource.Pause();
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
                mSoundHeap.Push(this);
                SoundManager.Instance.CurSounds.Remove(this);
            }
               
        }

        public void OnInit(object[] parmas = null)
        {
            mCurAudioSource = parmas[3] as AudioSource;
            mSoundHeap = parmas[4] as HeapScriptsPool<Sound>;
        }

        public void Pop(object[] parmas = null)
        {
            Play(parmas[0] as AudioClip, Convert.ToBoolean(parmas[1]), Convert.ToSingle(parmas[2]));
        }

        public void Push()
        {
            Res.ReleaseAsset<AudioClip>(mCurAudioClip);
        }

        public void FadeStop()
        {
           DG.Tweening.DOTween.To(() => mCurAudioSource.volume, x => mCurAudioSource.volume = x, 0, 0.3f).OnComplete(()=> 
           {
               Stop();
           });
        }
    }
}
