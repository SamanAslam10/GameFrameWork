using GameFrameWork.Movements;
using GameFrameWork.Properties;
using Microsoft.VisualBasic.Devices;
using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace GameFrameWork
{
    public partial class GameForm : Form
    {
        Game game = new Game();
        SoundManager sound = new SoundManager();
        PhysicsSystem physics = new PhysicsSystem();
        CollisionSystem collisions = new CollisionSystem();
        Random Random = new Random();

        private bool isLoading = true;
        private Panel loadingscreen;
        private ProgressBar loadingbar;
        private Panel gameOver;

        private string selectedPlantType = null;
   
        private DateTime lastUpdateTime = DateTime.Now;
        private Panel gamePanel;

        private Label sunCount = new Label();
        private int sunvalue = 0;
        private string PlayerName;
        private const int SUNFLOWER_COST = 50;
        private const int PEASHOOTER_COST = 100;
        Button sunflowerbtn;
        Button peashooterbtn;

        private bool levelStart = false;
        private int level = 0;
        private int levelTimer = 0;
        private int levelDuration = 0;

        private int maxZombie = 0;
        private int noZombieGenerated = 0;
        private int zombieGenerationTimer = 0;
        private int zombieGenerationDuration = 0;

        private string name;
        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            //1
            GameTimer.Interval = 16;
            GameTimer.Start();

            BackgroundMusic();
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
                    Login();
                }
                return;
            }
            DateTime currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - lastUpdateTime).TotalSeconds;
            lastUpdateTime = currentTime;
           
            if (levelStart && noZombieGenerated < maxZombie)
            {
                zombieGenerationTimer++;

                if (zombieGenerationTimer >= zombieGenerationDuration)
                {
                    
                    GenerateZombie();
                    zombieGenerationTimer = 0;
                    noZombieGenerated++;
                }
            }
            if (levelStart == true ) 
            {
                levelTimer++;
                PlantCardLock();
                CheckLevelCompletion();
                UpdateSunCount();
                sunCountLabel();
            }
            game.Update(new GameTime() { DeltaTime = deltaTime});
            if (gamePanel != null && game.Objects.Count > 0)
            {
                gamePanel.Invalidate();
            }
            physics.Apply(game.Objects.ToList());
            collisions.Check(game.Objects.ToList());
            game.Cleanup();
        }
        private void UpdateSunCount() 
        {
            sunvalue = game.sunCount;
        }
        private void GenerateZombie()
        {
            int y = 150 + Random.Next(0, 5) * 100;
            if (noZombieGenerated == maxZombie - 2) 
            {
                game.AddObject(new Enemy
                {
                    Position = new PointF(gamePanel.Width, y),
                    Size = new SizeF(250, 250),
                    Sprite = new AnimatedSprite(Resources.FlagZombieWalking),
                    Movement = new MoveLeftMovement(3f),
                    IsRigidBody = true,
                });
            }
            game.AddObject(new Enemy
            {
                Position = new PointF(gamePanel.Width, y),
                Size = new SizeF(250, 250),
                Sprite = new AnimatedSprite(Resources.BasicZombieWalking),
                Movement = new MoveLeftMovement(3f),
                IsRigidBody = true,
            });
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
            StartLevel(1);
            LoadLevels(1, levelDuration, true);
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
            lvl2.Click += lvl2_Click;
            lvl3.Click += lvl3_Click;

            Button backbutton = BackButton(Resources.backButton, 720, 120);
            backbutton.Click += backbutton_Click;

            levels.Controls.Add(lvl1);
            levels.Controls.Add(lvl2);
            levels.Controls.Add(lvl3);

            this.Controls.Add(levels);
        }
        private void lvl1_Click(object sender, EventArgs e)
        {
            StartLevel(1);
            LoadLevels(1, levelDuration, true);
        }
        private void lvl2_Click(object sender, EventArgs e)
        {
            StartLevel(2);
            LoadLevels(2, levelDuration, true);
        }
        private void lvl3_Click(object sender, EventArgs e)
        {
            StartLevel(3);
            LoadLevels(3, levelDuration, true);
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
            button.BackgroundImageLayout = ImageLayout.Stretch;
            if (unlocked)
            {
                button.BackgroundImage = img;
            }
            else 
            {
                button.BackgroundImage = Resources.level_locked_;
            }
            button.Enabled = unlocked;
            return button;
        }
        private void backbutton_Click(object sender, EventArgs e)
        {
            MainMenu();
        }
        private void Level(int no , int duration) 
        {
            int levelNo = no;
            int levelDuration = duration;
            if(levelTimer > levelDuration) 
            {
                levelStart = false;
                GameOver();
            } 
        }
        private void GameOver() 
        {
            GameEventsSound(Resources.gameOverSound);
            gameOver = new Panel();
            gameOver.BackgroundImage = Resources.gameOverCard;
            gameOver.BackgroundImageLayout = ImageLayout.Zoom;
            gameOver.Size = new Size(600,600 );
            gameOver.Location = new Point(600 , 300);
            gameOver.BackColor = Color.Transparent;
            gameOver.BorderStyle = BorderStyle.None;
            
            SetDoubleBuffered(gameOver);

            Button nextLevelBtn = new Button();
            nextLevelBtn.Text = "Next Level";
            nextLevelBtn.Size = new Size(200, 80);
            nextLevelBtn.Location = new Point(gameOver.Width / 2 - 100, gameOver.Height / 2);
            nextLevelBtn.Click += nextlevelBtn_Click;
            

            gameOver.Controls.Add(nextLevelBtn);
            this.Controls.Add(gameOver);
            gameOver.BringToFront();


        }
        private void nextlevelBtn_Click(object? sender, EventArgs e)
        {
            level++;
            StartLevel(level);
            this.Controls.Remove(gameOver);
        }
        private void StartLevel(int levelno) 
        {
            level = levelno;
            levelStart = true;
            levelTimer = 0;
            game.AddSun(200);
            if (level == 1) 
            {
                levelDuration = 3000;
                maxZombie = 15;
            }
            else if(level == 2) 
            {
                levelDuration = 3500;
                maxZombie = 20;
            }
            else if (level == 3) 
            {
                levelDuration = 4000;
                maxZombie = 25;
            }
            noZombieGenerated = 0;
            zombieGenerationTimer = 0;
            zombieGenerationDuration = 200;
        }
        private void CheckLevelCompletion() 
        {
            if (!levelStart) 
            {
                return;
            }
            if (levelTimer >= levelDuration) 
            {
                levelStart = false;

                int unlockedlevel = FileHandling.Load();
                if(level > unlockedlevel) 
                {
                    FileHandling.Save(level);
                }
                GameOver();
            }
        }
        private void LoadLevels(int no , int duration , bool start)
        {
            if (start == true) 
            {
                Level(no , duration); Controls.Clear();

                gamePanel = new Panel();
                gamePanel.BackgroundImage = Resources.lawn;
                gamePanel.Dock = DockStyle.Fill;
                gamePanel.BackgroundImageLayout = ImageLayout.Stretch;

                SetDoubleBuffered(gamePanel);

                gamePanel.Paint += (s, e) =>
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

                GameEventsSound(Resources.gameStartSound);
                gamePanel.Controls.Add(sunflowerbtn);
                gamePanel.Controls.Add(peashooterbtn);
                gamePanel.Controls.Add(SunBar());
                gamePanel.Controls.Add(ZombieBar());
                gamePanel.Controls.Add(TopBarMenuButton());
                gamePanel.Controls.Add(sunCount);
                sunCount.BringToFront();

            }
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
                            GameRef = game,
                            IsRigidBody = true,
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
                            GameRef = game,
                            IsRigidBody= true,
                        });
                        game.AddSun(-PEASHOOTER_COST);
                    }
                }
                
            } 
        }
        private void PlantCardLock() 
        {
            
            if(game.sunCount >= SUNFLOWER_COST) 
            {
                sunflowerbtn.BackgroundImage = Resources.sunflowerBar;
                sunflowerbtn.Enabled = true;
            }
            else 
            {
                sunflowerbtn.BackgroundImage = Resources.sunflowerBar_disabled_;
                sunflowerbtn.Enabled = false;
            }            
            if(game.sunCount  >= PEASHOOTER_COST) 
            {
                peashooterbtn.BackgroundImage = Resources.peashooterBar;
                peashooterbtn.Enabled = true;
            }
            else 
            {
                peashooterbtn.BackgroundImage = Resources.peashooterBar_disabled_;
                peashooterbtn.Enabled = false;
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
        private void GameEventsSound(System.IO.UnmanagedMemoryStream soundplay)
        {
            sound.Play(soundplay);
        }
        private void sunCountLabel() 
        {
            sunCount.Text = sunvalue.ToString();
            sunCount.Font = new Font("Arial", 24, FontStyle.Bold);
            sunCount.ForeColor = Color.Black;
            sunCount.BackColor = Color.LightGoldenrodYellow;
            sunCount.Location = new Point(500, 18);
            sunCount.AutoSize = true;
           
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
        private void Login() 
        {
            Panel login = new Panel();
            login.BackgroundImage = Resources.EnterName;
            login.Size = new Size(600, 600);
            login.Location = new Point(600, 300);
            login.BackgroundImageLayout = ImageLayout.Zoom;

            SetDoubleBuffered(login);
            this.Controls.Add(login);
            LoginTextBox(login);
            int x = this.Width/ 2;
            int y = this.Height - 100;
            Button Okbtn = loginButton(Resources.OkButton, x, y);
            Button cancelBtn = loginButton(Resources.CancelButton, x , y);
            
            Okbtn.Click += Okbtn_Click;
            cancelBtn.Click += cancelBtn_Click;

            login.Controls.Add(Okbtn);
            login.Controls.Add(cancelBtn);

            this.Controls.Add(login);
            login.BringToFront();
        }
        private void Okbtn_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlayerName)) 
            {
                return;
            }
            else 
            {
                SaveNameintoFile(PlayerName);
                Controls.Clear();
                MainMenu();
            }
        }
        private void cancelBtn_Click(object? sender, EventArgs e)
        {
            Controls.Clear();
            MainMenu();
        }

        private void LoginTextBox(Panel login) 
        {
            TextBox nameText = new TextBox();
            nameText.BackColor = Color.Brown;
            nameText.Size = new Size(400, 20);
            nameText.Location = new Point(1000, 800);
            nameText.Font = new Font("Arial", 16);

            PlayerName = nameText.Text;
            login.Controls.Add(nameText);
        }
        private Button loginButton(Image img , int x , int y) 
        {
            Button login = new Button();
            login.BackgroundImage = img;
            login.Size = new Size(150, 50);
            login.Location = new Point(x,y);

            return login;
        }
        private void SaveNameintoFile(string PlayerName) 
        {

        }
        private void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo prop = typeof(Control).GetProperty
            ( "DoubleBuffered",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(control, true, null);
        }
    }
}