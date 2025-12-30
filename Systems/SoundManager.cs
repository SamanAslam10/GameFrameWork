using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Microsoft.VisualBasic.Devices;
using GameFrameWork.Properties;

namespace GameFrameWork
{
    internal class SoundManager
    {
        private SoundPlayer backGroundSound;
        private SoundPlayer Sound;
        
        public void BackgroundPlay(System.IO.UnmanagedMemoryStream sound) 
        {
            backGroundSound = new SoundPlayer(sound);
            backGroundSound.PlayLooping();
        }
        public void Play(System.IO.UnmanagedMemoryStream sound)
        {
            Sound = new SoundPlayer(sound);
            Sound.Play();
        }
        
    }
}