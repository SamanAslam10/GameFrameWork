using GameFrameWork.Movements;
using System.Runtime.Versioning;

namespace GameFrameWork
{
    public partial class GameForm : Form
    {
        Game game = new Game();
        PhysicsSystem physicsSystem = new PhysicsSystem();
        TeleportMovement teleport;
        VerticalPatrolMovement verticalPatrol;

        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            GameTimer.Start();
        }

        private void Setting()
        {
            BackColor = Color.PaleTurquoise;
            game.AddObject(new Player
            {
                Movement = verticalPatrol,
                Position = new PointF(500, 140),
                Size = new SizeF(120, 120),
                Sprite = GameFrameWork.Properties.Resources.ufospaceshooter
            });
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            game.Update(new GameTime());
            physicsSystem.Apply(game.Objects.ToList());
            Invalidate();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            teleport = new TeleportMovement(this.ClientSize.Width, this.ClientSize.Height);
            verticalPatrol = new VerticalPatrolMovement(0, this.ClientSize.Height);
            Setting();
        }
    }
}
