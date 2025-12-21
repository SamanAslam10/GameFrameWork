using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZInput;

namespace GameFrameWork.Movements
{
    internal class ZigZagMovement : IMovement
    {
        private int move = 20;
        public void Move(GameObject obj , GameTime gameTime) 
        {
            if (Keyboard.IsKeyPressed(Key.Space) && Keyboard.IsKeyPressed(Key.RightArrow)) 
            {
                obj.Position = new PointF(obj.Position.X + move , obj.Position.Y - move);
            }
            if (Keyboard.IsKeyPressed(Key.Space) && Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                obj.Position = new PointF(obj.Position.X-move, obj.Position.Y - move);
            }
        }
    }
}
