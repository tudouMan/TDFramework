using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.HeapPool;
using TDFramework.Resource;
using UnityEngine;

namespace TDFramework.Audio
{
    public class SoundManager:MonoSingleton<SoundManager>
    {
        private List<Sound> mCurSounds;
        private HeapScriptsPool<Sound> mSoundHeap;

        private SoundManager()
        {
            mCurSounds = new List<Sound>();
            mSoundHeap = new HeapScriptsPool<Sound>();
        }

        //BgSound
        private Sound mBgSound;

        public List<Sound> CurSounds { get => mCurSounds; set => mCurSounds = value; }

        public void PlaySound(string soundName, float vol)
        {
            PlayAudio(soundName, false, vol);
        }

        public void PlayMusic(string soundName, float vol)
        {
            if (mBgSound != null)
                mBgSound.FadeStop();
            PlayAudio(soundName, true, vol);
        }

        private void PlayAudio(string soundName, bool isLoop,float vol)
        {
            if (mSoundHeap.IsPoolSizeFull)
            {
                Res.LoadAssetAsync<AudioClip>(soundName, p => 
                {
                   Sound sound=mSoundHeap.Pop();
                   sound.Play(p, isLoop, vol);
                    if (isLoop)
                        mBgSound = sound;
                    mCurSounds.Add(sound);
                });

            }
            else
            {
                Res.LoadAssetAsync<AudioClip>(soundName, p =>
                {
                    GameObject newObj = new GameObject(soundName);
                    newObj.transform.SetParent(this.transform);
                    AudioSource source= newObj.AddComponent<AudioSource>();
                    Sound sound=mSoundHeap.Pop();
                    sound.Play(p, isLoop, vol, source, mSoundHeap);
                    if (isLoop)
                        mBgSound = sound;
                    mCurSounds.Add(sound);
                });
            }
        }


        private void Update()
        {
            for (int i = mCurSounds.Count-1; i >=0 ; i--)
            {
                if (mCurSounds[i].IsFinish)
                {
                    mCurSounds[i].Stop();
                }
            }
        }
    }
}
