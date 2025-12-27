using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class AnimatedSprite : ISprite
    {
        private Image gif;
        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame;
        private float frameTime;
        private float timer;

        public Size Size { get; set; }

        public AnimatedSprite(Image gif, float fps = 10f)
        {
            this.gif = gif;
            dimension = new FrameDimension(gif.FrameDimensionsList[0]);
            frameCount = gif.GetFrameCount(dimension);
            currentFrame = 0;
            frameTime = 1f / fps; // default 10 frames per second
            timer = 0;
        }

        // Call this every update, deltaTime in seconds
        public void Update(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= frameTime)
            {
                timer -= frameTime;
                currentFrame = (currentFrame + 1) % frameCount;
            }
        }

        // Call this in your Draw method
        public void Draw(Graphics g, PointF position, SizeF size)
        {
            gif.SelectActiveFrame(dimension, currentFrame);
            g.DrawImage(gif, new RectangleF(position.X, position.Y, size.Width, size.Height));
        }
    }
}
