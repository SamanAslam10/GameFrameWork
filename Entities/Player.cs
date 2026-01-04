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
        public Game GameRef ;
        public bool IsEating { get; set; } = false;
        public float EatTimer { get; set; } = 0f;
        public float EatDuration { get; set; } = 2f;
        

        // Movement strategy: demonstrates composition over inheritance.
        // Different movement behaviors can be injected (KeyboardMovement, PatrolMovement, etc.).
        public IMovement? Movement { get; set; }
        

        // Domain state
        public int Health { get; set; } = 100;

        /// Update the player: delegate movement to the Movement strategy (if provided) and then apply base update.
        /// Shows the Strategy pattern (movement behavior varies independently from Player class).
        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
            if (PlantType != "Eater" && PlantType != "Jumper")
            {
                fireTimer += gameTime.DeltaTime;

                if (fireTimer >= FireCooldown)
                {
                    fireTimer = 0f;
                    Shoot();
                }
            }
            if (PlantType == "Eater" && IsEating)
            {
                EatTimer += gameTime.DeltaTime;

                if (EatTimer >= EatDuration)
                {
                    IsEating = false;
                    EatTimer = 0f;
                    Sprite = new AnimatedSprite(Resources.eater);
                }

                return; 
            }
            
        }
        private void Shoot()
        {
            if (PlantType == "Sunflower")
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.sun),
                    Size = new SizeF(130, 130),
                    Position = new PointF(Position.X, Position.Y-5),
                    Movement = new UpwardMovement(10f),
                    IsSun = true,
                    IsRigidBody = false,
                    gameRef = this.GameRef
                });
            }
            else if (PlantType == "Peashooter")
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.pea),
                    Size = new SizeF(60, 60),
                    Position = new PointF(Position.X + Size.Width, Position.Y),
                    Movement = new MoveRightMovement(10f),
                    IsRigidBody = false
                });
            }
            else if (PlantType == "Cactus") 
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.spikeBullet_),
                    Size = new SizeF(60, 60),
                    Position = new PointF(Position.X + Size.Width, Position.Y),
                    Movement = new MoveRightMovement(10f),
                    IsRigidBody = false
                });
            }
            else if (PlantType == "cattai")
            {
                GameRef.AddObject(new Bullet
                {
                    Sprite = new StaticSprite(Resources.spikeBullet_),
                    Size = new SizeF(60, 60),
                    Position = new PointF(Position.X + Size.Width, Position.Y),
                    Movement = new MoveRightMovement(10f),
                    IsRigidBody = false
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
            if (other is not Enemy zombie) return;
            
            if (PlantType == "Eater")
            {
                if (IsEating) return;
                if (zombie.Position.X < Position.X) return;

                IsEating = true;
                EatTimer = 0f;

                zombie.Health = 0;
                zombie.Movement = null;

                Sprite = new AnimatedSprite(Resources.eater_eating);

                return;
            }

            Health -= 1;
            zombie.IsEating = true;

            if (Health <= 0)
            {
                GameRef.EnemyScore += 10;
                IsActive = false;

                zombie.IsEating = false;
                zombie.Sprite = new AnimatedSprite(Resources.BasicZombieWalking);
                zombie.Movement = new MoveLeftMovement(3f);
            }
        }
    }
}