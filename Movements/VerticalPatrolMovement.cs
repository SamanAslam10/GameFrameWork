using GameFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class VerticalPatrolMovement : IMovement
    {
        private float upBound;
        private float downBound;
        private float speed = 2f;

        public VerticalPatrolMovement(float upBound , float downBound) 
        {
            this.upBound = upBound;
            this.downBound = downBound;
        }
        public void Move(GameObject obj, GameTime gameTime) 
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + speed);
            if (obj.Position.Y <= upBound) 
            {
                obj.Position = new PointF(obj.Position.X, upBound);
                speed = Math.Abs(speed); // move Down
            }
            if (obj.Position.Y >= downBound) 
            {
                obj.Position = new PointF(obj.Position.X, downBound);
                speed = -Math.Abs(speed); // move Up
            }
        }
    }
}
