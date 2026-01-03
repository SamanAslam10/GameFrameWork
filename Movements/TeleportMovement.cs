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
        private float speed;
        private int tickCounter = 0;

        public TeleportMovement(float speed)
        {
            this.speed = speed;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            tickCounter++;
            obj.Position = new PointF(obj.Position.X - speed, obj.Position.Y);

            if (tickCounter % 300 == 0) 
            {
                obj.Position = new PointF(obj.Position.X - 100, obj.Position.Y);
            }
        }
    }
}