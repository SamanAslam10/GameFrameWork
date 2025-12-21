using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class ChasingMovement : IMovement
    {
        GameObject chased = new GameObject();
        float speed = 2f;
        public ChasingMovement(GameObject obj)
        {
            this.chased = obj;
        }
       
        public void Move(GameObject obj , GameTime gameTime) 
        {
            float distanceX = chased.Position.X - obj.Position.X;
            float distanceY = chased.Position.Y - obj.Position.Y;

            float distance = (float)Math.Sqrt(distanceX*distanceX + distanceY*distanceY);
            if (distance > 0) 
            {
                float dX = distanceX/distance;
                float dY = distanceY/distance;

                float move = speed * (float)gameTime.DeltaTime;

                obj.Position = new PointF
                (
                    obj.Position.X + dX * move,
                    obj.Position.Y + dY * move
                );
            }
        }
    }
}
