using EZInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.Movements
{
    internal class JumpMovement : IMovement
    {
        private float jumpPower = 20f;
        public void Move(GameObject obj, GameTime gameTime)
        {
            if (Keyboard.IsKeyPressed(Key.Space) && Keyboard.IsKeyPressed(Key.Control))
            {
                obj.Velocity = new PointF(obj.Velocity.X, -jumpPower);
                obj.HasPhysics = true;
            }
        }
    }
}
