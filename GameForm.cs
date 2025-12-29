using GameFrameWork.Movements;
using GameFrameWork.Properties;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

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
   
        private DateTime lastUpdateTime = DateTime.Now;
        Panel gamePanel;

        private int score = 0;
        private Label scoreLabel;
        private float zombieSpawnTimer = 0f;
        private float zombieSpawnInterval = 3f;
        private int zombiesSpawned = 0;
        private int maxZombies = 10;

        private const int SUNFLOWER_COST = 50;
        private const int PEASHOOTER_COST = 100;
        Button sunflowerbtn;
        Button peashooterbtn;

        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            //1
            GameTimer.Interval = 16;
            GameTimer.Start();

            BackgroundMusic();
        }
        private void Setting()
        {
            game.AddObject(new Enemy
            {
                Position = new PointF(800, 900),
                Size = new Size(300, 300),
                Sprite = new AnimatedSprite(Resources.EatingFlagZombie,10f),
                Movement = new MoveLeftMovement(10f)
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
            //3
            DateTime currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - lastUpdateTime).TotalSeconds;
            lastUpdateTime = currentTime;

            PlantCardLock();
            game.Update(new GameTime() { DeltaTime = deltaTime});
            if (gamePanel != null && game.Objects.Count > 0)
            {
                gamePanel.Invalidate();
            }
            game.Cleanup();

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

            SetDoubleBuffered(loadingscreen);

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

            Button backbutton = BackButton(Resources.backButton, 720, 120);
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
            button.BackgroundImage = unlocked ? img : Resources.level_locked_;

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
            //
            gamePanel = new Panel();
            gamePanel.BackgroundImage = Resources.lawn;
            gamePanel.Dock = DockStyle.Fill;
            gamePanel.BackgroundImageLayout = ImageLayout.Stretch;

            SetDoubleBuffered(gamePanel);

            gamePanel.Paint += (s,e) => 
            {
                game.Draw(e.Graphics);
            };

            this.Controls.Add(gamePanel);
            gamePanel.SendToBack();

            game.Objects.Clear();

            sunflowerbtn = plantBarButtons(Resources.sunflowerBar, 20);
            sunflowerbtn.Click += Sunflowerbtn_Click;

            peashooterbtn = plantBarButtons(Resources.peashooterBar, 300);
            peashooterbtn.Click += Peashooterbtn_Click;

            gamePanel.MouseClick += gamePanel_MouseClick;

            
            gamePanel.Controls.Add(sunflowerbtn);
            gamePanel.Controls.Add(peashooterbtn);
            gamePanel.Controls.Add(SunBar());
            gamePanel.Controls.Add(ZombieBar());
            gamePanel.Controls.Add(TopBarMenuButton());
            gamePanel.Controls.Add(sunCountLabel());
            sunCountLabel().BringToFront();

            Setting();
        }
        private void gamePanel_MouseClick(object? sender, MouseEventArgs e)
        {
            if(selectedPlantType != null) 
            {
                
                if (selectedPlantType == "Sunflower") 
                {
                    if(game.sunCount < SUNFLOWER_COST) 
                    {
                        return;
                    }
                    if(game.sunCount >= SUNFLOWER_COST) 
                    {
                        game.AddObject(new Player
                        {
                            Sprite = new AnimatedSprite(Resources.sunflower),
                            Size = new SizeF(150, 150),
                            Position = new PointF(e.X, e.Y),
                            PlantType = "Sunflower",
                            FireCooldown = 5f,
                            GameRef = game
                        });
                        game.AddSun(-SUNFLOWER_COST);
                    }
                    
                }
                else if(selectedPlantType == "Peashooter") 
                {
                    if(game.sunCount < PEASHOOTER_COST) 
                    {
                        return;
                    }
                    if(game.sunCount >= PEASHOOTER_COST) 
                    {
                        game.AddObject(new Player
                        {
                            Sprite = new AnimatedSprite(Resources.Peashooter),
                            Size = new SizeF(200, 200),
                            Position = new PointF(e.X, e.Y),
                            PlantType = "Peashooter",
                            FireCooldown = 2f,
                            GameRef = game
                        });
                        game.AddSun(-PEASHOOTER_COST);
                    }
                }
                
            } 
        }
        private void PlantCardLock() 
        {
            if(selectedPlantType == "Sunflower") 
            {
                if(game.sunCount < SUNFLOWER_COST) 
                {
                    sunflowerbtn = plantBarButtons(Resources.sunflowerBar_disabled_, 20);
                    sunflowerbtn.Enabled = false;
                }
                else 
                {
                    sunflowerbtn = plantBarButtons(Resources.sunflowerBar, 20);
                    sunflowerbtn.Enabled = true;
                }
            }
            if(selectedPlantType == "Peashooter")
            {
                if(game.sunCount  < PEASHOOTER_COST) 
                {
                    peashooterbtn = plantBarButtons(Resources.peashooterBar_disabled_, 300);
                    peashooterbtn.Enabled = false;
                }
                else 
                {
                    peashooterbtn = plantBarButtons(Resources.peashooterBar_disabled_, 300);
                    peashooterbtn.Enabled = true;
                }
            }
        }
        private void Peashooterbtn_Click(object? sender, EventArgs e)
        {
            selectedPlantType = "Peashooter";
        }
        private void Sunflowerbtn_Click(object? sender, EventArgs e)
        {
            selectedPlantType = "Sunflower";
        }
        private Button plantBarButtons(Image img , int y) 
        {
            Button btn = new Button();
            btn.Size = new Size(150, 200);
            btn.Location = new Point(35, y);
            btn.BackgroundImage = img;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.BackColor = Color.FromArgb(180, 150, 90);

            return btn;
        }
        private void BackgroundMusic()
        {
            sound.BackgroundPlay(Resources.BackgroundSound);
        }
        private void LoadLevels2()
        {

            zombiesSpawned = 0;
            zombieSpawnTimer = 0f;
        }
        private Label sunCountLabel() 
        {
            scoreLabel = new Label
            {
                Text = game.sunCount.ToString(),
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(600, 10),
                AutoSize = true
            };
            return scoreLabel;
        }
        private Panel SunBar() 
        {
            Panel sunPanel = new Panel();
            sunPanel.BackgroundImage = Resources.sunbar;
            sunPanel.BackgroundImageLayout = ImageLayout.Stretch;
            sunPanel.Size = new Size(350, 80);
            sunPanel.Location = new Point(350, 20);
            sunPanel.BackColor = Color.Transparent; 
            sunPanel.BorderStyle = BorderStyle.None;

            SetDoubleBuffered(sunPanel);

            return sunPanel;
        }
        private ProgressBar ZombieBar() 
        {
            ProgressBar zombieBar = new ProgressBar();
            zombieBar.Size = new Size(250, 50);
            zombieBar.Location = new Point(800, 20);

            zombieBar.Value = ZombieBarValue();
            return zombieBar;
        }
        private int ZombieBarValue() 
        {
            return 50;
        }
        private Button TopBarMenuButton() 
        {
            Button menuBtn = new Button();
            menuBtn.Image = Resources.menubar;
            menuBtn.BackgroundImageLayout =(ImageLayout)ImageLayout.Stretch;
            menuBtn.Size = new Size(250,50);
            menuBtn.Location = new Point(1400, 20);
            menuBtn.BackColor = Color.Transparent;
            menuBtn.FlatStyle = FlatStyle.Flat;
            menuBtn.FlatAppearance.BorderSize = 0;

            menuBtn.Click += menubtn_Click;

            return menuBtn;
        }
        private void menubtn_Click(object? sender, EventArgs e)
        {
            Controls.Clear();
            MainMenu();
        }
        private void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo prop = typeof(Control).GetProperty
                ( "DoubleBuffered",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                );

            prop.SetValue(control, true, null);
        }
    }
}