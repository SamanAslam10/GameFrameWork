using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork.Movements
{
    internal class MoveRightMovement : IMovement
    {
        float speed;

        public MoveRightMovement(float speed)
        {
            this.speed = speed;
        }
        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X + speed, obj.Position.Y);
        }
    }
}
