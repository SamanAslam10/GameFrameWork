using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZInput;

namespace GameFrameWork.Movements
{
    internal class TeleportMovement : IMovement
    {
        Random random = new Random();
        private float width;
        private float height; 
        public TeleportMovement(float width , float height) 
        {
            this.width = width;
            this.height = height;
        }   
        public void Move(GameObject obj , GameTime gameTime) 
        {
            if (Keyboard.IsKeyPressed(Key.Shift)) 
            {
                obj.Position = RandomPosition(obj.Size);
            }
        }
        private PointF RandomPosition(SizeF size) 
        {
            float x = random.Next(0, (int)(width - size.Width));
            float y = random.Next(0, (int)(height - size.Height));
            return new PointF(x, y);
        }
    }
}