using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZInput;

namespace PlantsVsZombies.Movements
{
    internal class DiagonalJumpMovement : IMovement
    {
        private int jump = 20;
        public void Move(GameObject obj , GameTime gameTime) 
        {
            if (Keyboard.IsKeyPressed(Key.Space) && Keyboard.IsKeyPressed(Key.RightArrow)) 
            {
                obj.Position = new PointF(obj.Position.X + jump , obj.Position.Y - jump);
            }
            if (Keyboard.IsKeyPressed(Key.Space) && Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                obj.Position = new PointF(obj.Position.X-jump, obj.Position.Y - jump);
            }
        }
    }
}
