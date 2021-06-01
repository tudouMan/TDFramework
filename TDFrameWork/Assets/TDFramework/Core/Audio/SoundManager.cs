using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.Resource;
using UnityEngine;

namespace TDFramework.Audio
{
    public class SoundManager : ManagerBase, IDisposable
    {
        private List<Sound> mCurSounds=new List<Sound>();

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

            GameEntry.Res.LoadAssetAsync<AudioClip>(soundName, p =>
                {
                    Sound sound = GameEntry.Pool.PopClass<Sound>();
                    sound.Play(p, isLoop, vol);
                    if (isLoop)
                        mBgSound = sound;
                    mCurSounds.Add(sound);
                });
        }

        public void Remove(Sound sound)
        {
            CurSounds.Remove(sound);
        }

        public void Update()
        {
            for (int i = mCurSounds.Count-1; i >=0 ; i--)
            {
                if (mCurSounds[i].IsFinish)
                {
                    mCurSounds[i].Stop();
                }
            }
        }

        internal override void Init()
        {
            
        }

        public void Dispose()
        {
           
        }
    }
}
