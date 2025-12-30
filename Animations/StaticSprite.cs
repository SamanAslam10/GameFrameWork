using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class StaticSprite : ISprite
    {
        private Image image;

        public StaticSprite(Image img)
        {
            image = img;
        }

        public void Update(GameTime gameTime) {}

        public void Draw(Graphics g, PointF position, SizeF size)
        {
            g.DrawImage(image , new RectangleF(position.X, position.Y, size.Width, size.Height));
        }
    }
}
