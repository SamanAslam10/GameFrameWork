using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    public class AnimationSystem
    {
        private Dictionary<String, Image[]> Animations;
        int CurrentFrame;
        string CurrentState;
        float FrameDuration;
        float timer;
        
        public void AddAnimation(string state, Image[] frames ,float frameDuration) 
        {
            Animations[state] = frames;
            FrameDuration = frameDuration;
        }
        public void UpdateFrame(GameTime gameTime) 
        {
            timer = gameTime.DeltaTime;
            if ( timer > FrameDuration) 
            {
                CurrentFrame = (CurrentFrame + 1) % Animations[CurrentState].Length;
                timer = 0;
            }
        }
        public void GetState(string state) 
        {
            if (CurrentState != state)
            {
                CurrentState = state;
                CurrentFrame = 0;
                timer = 0;
            }
        }
        public Image GetAnimatations() 
        {
            return Animations[CurrentState][CurrentFrame];
        }
    }
}