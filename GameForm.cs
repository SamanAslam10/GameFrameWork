using GameFrameWork.Movements;
using GameFrameWork.Properties;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Policy;

namespace GameFrameWork
{
    public partial class GameForm : Form
    {
        Game game = new Game();
        PhysicsSystem physicsSystem = new PhysicsSystem();
        AnimatedSprite animatedSprite;
        StaticSprite staticSprite;
        Player player;
        Enemy enemy;
        SoundManager sound = new SoundManager();
        Random Random = new Random();
        MoveLeftMovement move;

        private bool isLoading = true;
        private int loadingProgress = 0;
        private Panel loadingscreen;
        private ProgressBar loadingbar;

        private string selectedPlantType = null;
        


        private int score = 0;
        private Label scoreLabel;
        private float zombieSpawnTimer = 0f;
        private float zombieSpawnInterval = 3f;
        private int zombiesSpawned = 0;
        private int maxZombies = 10;


        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            GameTimer.Start();
            BackgroundMusic();
        }
        private void Setting()
        {
            game.AddObject(new Player
            {
                Position = new PointF(100, 200),
                Size = new Size(100, 100),
                Sprite = new AnimatedSprite(Resources.EatingFlagZombie),
                Movement = new MoveLeftMovement(5f)
            });

        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isLoading)
            {
                loadingbar.Value += 1;

                if (loadingbar.Value >= 100)
                {
                    isLoading = false;
                    this.Controls.Remove(loadingscreen);
                    MainMenu();
                }
                return;
            }

            physicsSystem.Apply(game.Objects.ToList());
            foreach (var obj in game.Objects)
            {
                game.Update(new GameTime());
            }
            game.Cleanup();
            Invalidate();

        }
        private void GameForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;


            LoadingScreen();
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

            Button StartButton = CreateMenuButton("START", centerX, baseY);
            Button LevelButton = CreateMenuButton("LEVELS", centerX, baseY + gap);
            Button ExitButton = CreateMenuButton("EXIT", centerX, baseY + gap * 2);

            StartButton.Click += StartButton_Click;
            LevelButton.Click += LevelButton_Click;
            ExitButton.Click += ExitButton_Click;

            mainMenu.Controls.Add(StartButton);
            mainMenu.Controls.Add(LevelButton);
            mainMenu.Controls.Add(ExitButton);


            this.Controls.Add(mainMenu);
            mainMenu.BringToFront();
        }
        private Button CreateMenuButton(string Text, int x, int y)
        {
            return new Button
            {
                Text = Text,
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
            LoadLevels();
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

            lvl1.Click += lvl1_Click;

            Button backbutton = BackButton(Resources.backButton, 720, 1200);
            backbutton.Click += backbutton_Click;

            levels.Controls.Add(lvl1);
            levels.Controls.Add(lvl2);
            levels.Controls.Add(lvl3);

            this.Controls.Add(levels);
        }
        private void lvl1_Click(object sender, EventArgs e)
        {
            LoadLevels();
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
        private Button CreateLevelButton(Image img, int x, int y, bool unlocked)
        {
            Button button = new Button();
            button.Size = new Size(280, 260);
            button.Location = new Point(x, y);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(-2);
            button.BackgroundImage = img;
            button.BackgroundImageLayout = ImageLayout.Stretch;
            button.BackgroundImage = unlocked ? img : Resources.levelLocked;

            button.Enabled = unlocked;
            return button;
        }
        private void backbutton_Click(object sender, EventArgs e)
        {
            MainMenu();
        }
        private void LoadLevels()
        {
            Controls.Clear();
            Panel BackGround = new Panel();
            BackGround.BackgroundImage = Resources.lawn;
            BackGround.Dock = DockStyle.Fill;
            BackGround.BackgroundImageLayout = ImageLayout.Zoom;

            this.Controls.Add(BackGround);
            BackGround.SendToBack();

            BackGround.Paint += BackGround_Paint;

            game.Objects.Clear();
            CreatePlants();
        }
        private void CreatePlants() 
        {
            Panel bar = PlantSelectionBar();
            bar.MouseClick += Bar_MouseClick;

            this.Controls.Add(bar);
            bar.BringToFront();
        }

        private void Bar_MouseClick(object? sender, MouseEventArgs e)
        {
            if(selectedPlantType != null) 
            {
                Image plantSprite = null;
                if (selectedPlantType == "Sunflower") 
                {
                    plantSprite = Resources.sunflower;
                }
                else if(selectedPlantType == "Peashooter") 
                {
                    plantSprite = Resources.Peashooter;

                }
                if (plantSprite != null) 
                {
                    game.AddObject(new Player
                    {
                        Sprite = new AnimatedSprite(plantSprite),
                        Size = new SizeF(200, 200),
                        Position = new PointF(e.X, e.Y)
                    });
                }
            } 
        }

        private Panel PlantSelectionBar() 
        {
            Panel plantBar = new Panel();
            plantBar.Dock = DockStyle.Top;
            plantBar.Height = 200;
            plantBar.BackColor = Color.FromArgb(61, 48, 39);

            Button sunflowerbtn = plantBarButtons(Resources.Sunflowerlogo);
            sunflowerbtn.Click += Sunflowerbtn_Click;

            Button peashooterbtn = plantBarButtons(Resources.peashooterlogo);
            peashooterbtn.Click += Peashooterbtn_Click;

            plantBar.Controls.Add(sunflowerbtn);
            plantBar.Controls.Add(peashooterbtn);
            return plantBar;
        }

        private void Peashooterbtn_Click(object? sender, EventArgs e)
        {
            selectedPlantType = "Peashooter";
        }

        private void Sunflowerbtn_Click(object? sender, EventArgs e)
        {
            selectedPlantType = "Sunflower";
        }

        private Button plantBarButtons(Image img ) 
        {
            Button button = new Button();
            button.Size = new Size(180, 180);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(-2);
            button.BackgroundImage = img;
            button.BackgroundImageLayout = ImageLayout.Stretch;

            return button;
        }
        private void BackGround_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }
        private void BackgroundMusic()
        {
            sound.BackgroundPlay(Resources.BackgroundSound);
        }
        private void LoadLevels2()
        {

            zombiesSpawned = 0;
            zombieSpawnTimer = 0f;

            score = 0;
            scoreLabel = new Label
            {
                Text = "Score: 0",
                Font = new Font("Arial", 24),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);
        }
    }
}