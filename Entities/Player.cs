using GameFrameWork.Movements;
using GameFrameWork.Properties;
using System.Drawing;
namespace GameFrameWork
{
    public class Player : GameObject
    {
        public float FireCooldown ;
        private float fireTimer = 0f;
        public string PlantType;

        // Movement strategy: demonstrates composition over inheritance.
        // Different movement behaviors can be injected (KeyboardMovement, PatrolMovement, etc.).
        public IMovement? Movement { get; set; }
        

        // Domain state
        public int Health { get; set; } = 100;
        public int Score { get; set; } = 0;

        /// Update the player: delegate movement to the Movement strategy (if provided) and then apply base update.
        /// Shows the Strategy pattern (movement behavior varies independently from Player class).
        public override void Update(GameTime gameTime)
        {
            fireTimer += gameTime.DeltaTime;

            if (fireTimer >= FireCooldown)
            {
                fireTimer = 0f;
                Shoot();
            }
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
        }
        private void Shoot()
        {
            if (PlantType == "Sunflower")
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.sun),
                    Size = new SizeF(60, 60),
                    Position = new PointF(Position.X + 30, Position.Y + 30),
                    Movement = new UpwardMovement(10f),
                    IsSun = true,

                });
            }
            else if (PlantType == "Peashooter")
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.pea),
                    Size = new SizeF(40, 40),
                    Position = new PointF(Position.X + Size.Width, Position.Y + 40),
                    Movement = new MoveRightMovement(10f)
                });
            }
        }
        /// Draw uses base implementation; override if player needs custom visuals.

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        /// Collision reaction for the player. Demonstrates single responsibility: domain reaction is handled here.
        public override void OnCollision(GameObject other)
        {
            if (other is Enemy zombie)
            {
                Health -= 1;
                zombie.IsEating = true;

                if (Health <= 0)
                {
                    IsActive = false;
                    zombie.IsEating = false;
                    zombie.Sprite = new AnimatedSprite(GameFrameWork.Properties.Resources.BasicZombieWalking);
                }
            }
        }
    }
 
}