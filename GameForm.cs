using GameFrameWork.Movements;
using GameFrameWork.Properties;
using System.Drawing;
using System.Runtime.CompilerServices;
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

            StartButton.Click += StartButton_Click;
            LevelButton.Click += LevelButton_Click;
            ExitButton.Click += ExitButton_Click;

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
        private void StartButton_Click(object sender, EventArgs e) 
        {

        }
        private void LevelButton_Click(object sender, EventArgs e)
        {
            LevelMenu();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }
        private void LevelMenu() 
        {
            Controls.Clear();
            Panel levels = new Panel();
            levels.BackgroundImage = Resources.levelsMenu;
            levels.Dock = DockStyle.Fill;
            levels.BackgroundImageLayout = ImageLayout.Stretch;

            int unlocked = FileHandling.Load();

            int x = 350;
            int gap = 150;
            int width = 280;
            int y = 120;

            Button lvl1 = CreateLevelButton(Resources.level1, x, y, unlocked >= 1);
            Button lvl2 = CreateLevelButton(Resources.level2, x + (gap + width), y, unlocked >= 2);
            Button lvl3 = CreateLevelButton(Resources.level3, x + (gap + width) * 2, y, unlocked >= 3);

            Button backbutton = BackButton(Resources.backButton, 720, 1200);
            backbutton.Click += backbutton_Click;

            levels.Controls.Add(lvl1);
            levels.Controls.Add(lvl2);
            levels.Controls.Add(lvl3);

            this.Controls.Add(levels);
        }
        private Button BackButton(Image img, int x, int y)
        {
            Button button = new Button();
            button.Size = new Size(280, 260);
            button.Location = new Point(x, y);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(-2);
            button.Image = img;
            return button;
        }
        private Button CreateLevelButton(Image img ,int x , int y , bool unlocked) 
        {
            Button button = new Button();
            button.Size = new Size(280, 260);
            button.Location = new Point(x, y);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(-2);
            button.Image = img;
            button.Image = unlocked ? img : Resources.levelLocked;

            button.Enabled = unlocked;
            return button;
        }
        private void backbutton_Click(object sender, EventArgs e)
        {
            MainMenu();
        }
    }
}