using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    public interface ISprite
    {
        void Update(GameTime gameTime);
        void Draw(Graphics g , PointF position , SizeF size);
    }
}
