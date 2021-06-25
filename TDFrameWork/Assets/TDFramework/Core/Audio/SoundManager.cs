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
        private List<Sound> m_CurSounds=new List<Sound>();

        //BgSound
        private Sound m_BgSound;

        public List<Sound> CurSounds { get => m_CurSounds; set => m_CurSounds = value; }

        public void PlaySound(string soundName, float vol)
        {
            PlayAudio(soundName, false, vol);
        }

        public void PlayMusic(string soundName, float vol)
        {
            if (m_BgSound != null)
                m_BgSound.FadeStop();
            PlayAudio(soundName, true, vol);
        }

        private void PlayAudio(string soundName, bool isLoop,float vol)
        {
            GameEntry.Res.LoadAssetAsync<AudioClip>(soundName, p =>
                {
                    Sound sound = GameEntry.Pool.PopClass<Sound>();
                    sound.Play(p, isLoop, vol);
                    if (isLoop)
                        m_BgSound = sound;
                    m_CurSounds.Add(sound);
                });
        }


        public void Remove(Sound sound)
        {
            CurSounds.Remove(sound);
        }

        public void Update()
        {  
            for (int i = m_CurSounds.Count-1; i >=0 ; i--)
            {
                if (m_CurSounds[i].IsFinish)
                {
                    m_CurSounds[i].Stop();
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
