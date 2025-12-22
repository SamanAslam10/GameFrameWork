using Microsoft.VisualBasic.ApplicationServices;
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
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private float speed = 60f;
        bool hasTarget = false;
        PointF targetPoints;

        public RandomPatrol(float left, float right, float top, float bottom)
        {
            this.minX = left;
            this.maxX = right;
            this.minY = top;
            this.maxY = bottom;
        }

        public void Move(GameObject obj , GameTime gameTime) 
        {
            if (hasTarget == false) 
            {
                PickTarget();
            }
            float distanceX = targetPoints.X - obj.Position.X;
            float distanceY = targetPoints.Y - obj.Position.Y;

            float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            if (distance < speed) 
            {
                hasTarget = false;
                return;
            }
            float directionX = distanceX / distance;
            float directionY = distanceY / distance;

            obj.Position = new PointF
            (
                obj.Position.X + directionX * speed * gameTime.DeltaTime,
                obj.Position.Y + directionY * speed * gameTime.DeltaTime
            );
        }
        private void PickTarget() 
        {
            targetPoints = new PointF
            (
                random.Next((int)minX,(int)maxX-10),
                random.Next((int)minY,(int)maxY - 10)
            );
            hasTarget = true;
        }

    }
}