using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class EnvironmentObject : IMovable, ICollidable, IPhysicsObject, IDrawable, IUpdatable
    {
        public string Type {  get; set; }
        public Image Sprite {  get; set; }
        public SizeF Size {  get; set; }

        public PointF position { get; set; }

        public bool IsRigidBody { get; set; } = false;
        public bool IsCollidable { get; set; } = false;

        public bool HasPhysics { get; set; } = false;

        public bool HasGravity { get; set; } = false;
        public bool IsMoveable { get; set; } = false;
        public PointF Velocity { get; set; } = PointF.Empty;
        public float? CustomGravity { get; set; } = null;

        public RectangleF Bounds => new RectangleF(position, Size);
        public EnvironmentObject(string Type , Image sprite , SizeF size) 
        {
            this.Type = Type;
            this.Sprite = sprite;
            this.Size = size;
        }

         public virtual void Draw(Graphics graphics)
        {
            if (Sprite != null)
            {
                graphics.DrawImage(Sprite, Bounds);
            }
            else
            {
                using (Brush brush = new SolidBrush(Color.Gray)) // Default color
                {
                    graphics.FillRectangle(brush, Bounds);
                }
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            position = new PointF(position.X + Velocity.X, position.Y + Velocity.Y);
        }
        public virtual void OnCollision(GameObject other)
        {
            // Default behavior: Do nothing
        }
    }
}
