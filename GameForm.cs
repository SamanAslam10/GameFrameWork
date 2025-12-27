using GameFrameWork.Movements;
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

        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            GameTimer.Start();
        }

        private void Setting()
        {
            
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
            
            Setting();
        }
    }
}
