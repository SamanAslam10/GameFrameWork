using GameFrameWork.Movements;
using GameFrameWork.Properties;
using System.Drawing;
using System.Runtime.Versioning;

namespace GameFrameWork
{
    public partial class GameForm : Form
    {
        Game game = new Game();
        PhysicsSystem physicsSystem = new PhysicsSystem();
        TeleportMovement teleport;
        RandomPatrol randomPatrol;
        VerticalPatrolMovement verticalPatrol;
        AnimatedSprite animatedSprite ;
        StaticSprite staticSprite;

        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            GameTimer.Start();
        }

        private void Setting()
        {
            game.AddObject(new Enemy
            {
                Sprite = new AnimatedSprite( Resources.EatingFlagZombie),
                Movement = verticalPatrol,
                Size = new SizeF(120 , 130),
                Position = new PointF(550 , 550)
            });
            game.AddObject(new Player
            {
                Position = new PointF(100, 200),
                Size = new Size(100, 100),
                Sprite = new StaticSprite(Resources.pea),
                Movement = new KeyboardMovement()
            });
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            game.Draw(e.Graphics);
            Graphics g = e.Graphics;
            foreach (var obj in game.Objects) 
            {
                obj.Draw(g);
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            game.Update(new GameTime());
            physicsSystem.Apply(game.Objects.ToList());

            float deltaTime = 1f / 60f; // 60 FPS example

            foreach (var obj in game.Objects)
                obj.Update(new GameTime());

            Invalidate();
            
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            
            Setting();
            verticalPatrol = new VerticalPatrolMovement(this.Top , this.Bottom);
        }
    }
}
