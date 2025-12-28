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
        private SoundPlayer winSound;
        private SoundPlayer loseSound;
        private SoundPlayer damageSound;
        private SoundPlayer scoreSound;
        
        public void BackgroundPlay(System.IO.UnmanagedMemoryStream sound) 
        {
            backGroundSound = new SoundPlayer(sound);
            backGroundSound.PlayLooping();
        }
        public void winPlay(string path)
        {
            winSound = new SoundPlayer(path);
            winSound.Play();
        }
        public void losePlay(string path)
        {
            loseSound = new SoundPlayer(path);
            loseSound.Play();
        }
        public void damagePlay(string path)
        {
            damageSound = new SoundPlayer(path);
            damageSound.Play();
        }
        public void scorePlay(string path)
        {
            scoreSound = new SoundPlayer(path);
            scoreSound.Play();
        }
    }
}