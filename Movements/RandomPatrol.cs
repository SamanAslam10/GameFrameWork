using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class RandomPatrol : IMovement
    {
        Random random = new Random();
        private float left;
        private float right;
        private float top;
        private float bottom;
        private float speed = 3f;

        public RandomPatrol(float left, float right, float top, float bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        public void Move(GameObject obj , GameTime gameTime) 
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + speed);
            if (obj.Position.Y <= top)
            {
                obj.Position = new PointF(obj.Position.X, top);
                speed = Math.Abs(speed); // move Down
            }
            if (obj.Position.Y >= bottom)
            {
                obj.Position = new PointF(obj.Position.X, bottom);
                speed = -Math.Abs(speed); // move Up
            }
            obj.Position = new PointF(obj.Position.X + speed, obj.Position.Y);

            if (obj.Position.X < left)
            {
                obj.Position = new PointF(left, obj.Position.Y);
                speed = Math.Abs(speed); // Move right
            }
            else if (obj.Position.X > right)
            {
                obj.Position = new PointF(right, obj.Position.Y);
                speed = -Math.Abs(speed); // Move left
            }
        }
    }
}