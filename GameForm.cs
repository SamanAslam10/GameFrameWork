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

        private bool isLoading = true;
        private int loadingProgress = 0;
        private Panel loadingscreen;
        private ProgressBar loadingbar;


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
            if (isLoading)
            {
                loadingbar.Value += 1; 

                if (loadingbar.Value >= 100 )
                {
                    isLoading = false;
                    this.Controls.Remove(loadingscreen);
                    MainMenu();
                }
                return; 
            }

            game.Update(new GameTime());
            physicsSystem.Apply(game.Objects.ToList());
            foreach (var obj in game.Objects) 
            {
                obj.Update(new GameTime());
            }


            Invalidate();
            
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;


            LoadingScreen();
            Setting();
            verticalPatrol = new VerticalPatrolMovement(this.Top , this.Bottom);
        }
        private void LoadingScreen() 
        {
            loadingscreen = new Panel();
            loadingscreen.BackgroundImage = Resources.main;
            loadingscreen.BackgroundImageLayout = ImageLayout.Stretch;
            loadingscreen.Size = this.ClientSize;
            loadingscreen.Location = new Point(0, 0);

            loadingbar = new ProgressBar();
            loadingbar.Show();
            loadingbar.Minimum = 0;
            loadingbar.Maximum = 100;
            loadingbar.Value = 0;
            loadingbar.Size = new Size(this.Width - 300, 30);
            loadingbar.Location = new Point(150, this.Height - 70);
            loadingbar.Style = ProgressBarStyle.Continuous;

            this.Controls.Add(loadingscreen);
            loadingscreen.Controls.Add(loadingbar);
            loadingscreen.BringToFront();
        }
        private void MainMenu() 
        {
            Panel mainMenu = new Panel();
            mainMenu.BackgroundImage = Resources.mainMenu;
            mainMenu.BackgroundImageLayout = ImageLayout.Stretch;
            mainMenu.Dock = DockStyle.Fill;

            int centerX = 1150;  
            int baseY = 320;   
            int gap = 145;

            Button StartButton = CreateMenuButton("START" , centerX, baseY);
            Button LevelButton = CreateMenuButton("LEVELS",centerX, baseY + gap);
            Button ExitButton = CreateMenuButton("EXIT",centerX, baseY + gap * 2);

            mainMenu.Controls.Add(StartButton);
            mainMenu.Controls.Add(LevelButton);
            mainMenu.Controls.Add(ExitButton);

            this.Controls.Add(mainMenu);
            mainMenu.BringToFront();
        }
        private Button CreateMenuButton(string Text , int x , int y) 
        {
            return new Button
            {
                Text = Text ,
                Font = new Font("Showcard Gothic", 32, FontStyle.Italic),
                ForeColor = Color.Black,
                BackColor = Color.DarkGray,
                Size = new Size(350, 101),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat
            };
        }
    }
}
