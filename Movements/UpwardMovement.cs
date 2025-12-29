using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class UpwardMovement : IMovement
    {
        float speed;

        public UpwardMovement(float speed)
        {
            this.speed = speed;
        }
        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y - speed);
        }
    }
}
