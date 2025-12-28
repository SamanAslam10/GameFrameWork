using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class MoveLeftMovement : IMovement
    {
        float speed;

        public MoveLeftMovement(float speed) 
        {
            this.speed = speed;
        }
        public void Move(GameObject obj , GameTime gameTime) 
        {
            obj.Position = new PointF(obj.Position.X - speed, obj.Position.Y);
        }
    }
}
