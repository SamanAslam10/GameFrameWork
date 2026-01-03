using EZInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork.Movements
{
    internal class JumpMovement : IMovement
    {
        private float speed;
        private float jumpHeight = 50f;
        private float groundY;
        private bool jumping = false;

        public JumpMovement(float speed, float startY)
        {
            this.speed = speed;
            this.groundY = startY;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X - speed, obj.Position.Y);

            if (!jumping && new Random().Next(0, 200) == 0)
            {
                jumping = true;
            }

            if (jumping)
            {
                obj.Position = new PointF(obj.Position.X, obj.Position.Y - jumpHeight * gameTime.DeltaTime);
                jumping = false;
            }
        }
    }
}