using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Windows.Media;
using System.Media;
using Brushes = System.Drawing.Brushes;

namespace test
{
    public class KillFlyGame
    {
        public FlyModel Model { get; set; }
        public FlyController Controller { get; set; }
        private Image FlyImage;
        private Bitmap BackgroundBitmap;
        private static MainForm mainForm;
        private SoundPlayer HitSound;
        private SoundPlayer MissSound;
        private MediaPlayer FlyNoice;
        

        public KillFlyGame(MainForm form)
        {
            mainForm = form;
            Model = new FlyModel();
            Controller = new FlyController(Model, form);
            MissSound = new SoundPlayer("KillFlyGame/Resources/Sounds/miss.wav");
            HitSound = new SoundPlayer("KillFlyGame/Resources/Sounds/hit.wav");
            FlyImage = Image.FromFile("KillFlyGame/Resources/Images/fly.png");
            BackgroundBitmap = new Bitmap(Image.FromFile("KillFlyGame/Resources/Images/background.jpg"));
        }

        public class FlyModel
        {
            public Point Center { get; set; }
            public int Size { get; set; }
            public bool IsGrowing { get; set; }
            public TimeSpan TimeLeft { get; set; }
            public int Score { get; set; }
            public int MaxSize = 50;
            public int MinSize = 1;
            public Font GameFont { get; set; }
        }

        public class FlyController
        {
            private FlyModel fly;
            private MainForm mainForm;
            private Random random = new Random();
            private Timer gameTimer;
            private Timer ballTimer;
            private DateTime startTime;

            public FlyController(FlyModel fly, MainForm form)
            {
                this.fly = fly;
                this.mainForm = form;

                fly.Center = GetRandomPosition();
                fly.Size = 1;
                fly.IsGrowing = true;
                fly.TimeLeft = TimeSpan.FromMinutes(1);
                fly.Score = 0;

                startTime = DateTime.Now;

                gameTimer = new Timer();
                gameTimer.Interval = 60000;
                gameTimer.Tick += (s, e) =>
                {
                    gameTimer.Stop();
                    ballTimer.Stop();
                    MessageBox.Show("Game Over");
                };
                gameTimer.Start();

                ballTimer = new Timer();
                ballTimer.Interval = 20;
                ballTimer.Tick += (s, e) =>
                {
                    if (fly.IsGrowing)
                    {
                        fly.Size++;
                        if (fly.Size >= 50)
                        {
                            fly.IsGrowing = false;
                            fly.Score -= 150;
                            if (fly.Score < 0)
                                fly.Score = 0;
                        }    

                    }
                    else
                    {
                        fly.Size = 1;
                        fly.Center = GetRandomPosition();
                        fly.IsGrowing = true;
                    }

                    var elapsed = DateTime.Now - startTime;
                    ballTimer.Interval = (int)(25 * (1 + elapsed.TotalSeconds / 60));
                    fly.TimeLeft = TimeSpan.FromMinutes(1) - elapsed;
                    if (fly.TimeLeft < TimeSpan.Zero)
                        fly.TimeLeft = TimeSpan.Zero;

                    form.Invalidate();

                };
                ballTimer.Start();
            }

            private Point GetRandomPosition()
            {
                return new Point(
                    random.Next(50, mainForm.ClientSize.Width - 50),
                    random.Next(50, mainForm.ClientSize.Height - 50));
            }

            private int CalculateScore()
            {
                var maxScore = 300;
                var minScore = 30;
                var scoreDecreaseRate = fly.Size / (double)fly.MaxSize;
                var score = maxScore - (int)(300 * scoreDecreaseRate);
                return Math.Max(score, minScore);
            }

            public void HandleMouseClick(object sender, MouseEventArgs e)
            {
                var flyRectangle = new Rectangle(new Point(fly.Center.X - fly.Size / 2, fly.Center.Y - fly.Size / 2), new Size(fly.Size, fly.Size));
                if (flyRectangle.Contains(e.Location))
                {
                    mainForm.KillFlyGame.HitSound.Play();
                    fly.Score += mainForm.KillFlyGame.Controller.CalculateScore();
                    fly.Size = 1;
                    fly.Center = mainForm.KillFlyGame.Controller.GetRandomPosition();
                    mainForm.Invalidate();
                }
                else
                {
                    mainForm.KillFlyGame.MissSound.Play();
                    fly.Score -= 75;
                    if (fly.Score < 0)
                        fly.Score = 0;
                }
            }
        }

        public void Draw(Graphics g)
        {
             
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(BackgroundBitmap, 0, 0);
            g.DrawImage(FlyImage, new Rectangle(new Point(Model.Center.X - Model.Size / 2, Model.Center.Y - Model.Size / 2), new Size(Model.Size, Model.Size)));
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile("Fonts/BetterVCR.ttf");
            var font = new Font(pfc.Families[0], 16, FontStyle.Bold);

            var timeText = Model.TimeLeft.ToString(@"ss");
            var textSize = g.MeasureString(timeText, font);
            var location = new PointF((mainForm.ClientSize.Width - textSize.Width) / 2, mainForm.ClientSize.Height - textSize.Height);
            g.DrawString(timeText, font, Brushes.White, location);

            var scoreText = "Очки: " + Model.Score.ToString();
            textSize = g.MeasureString(scoreText, font);
            location = new PointF(mainForm.ClientSize.Width - textSize.Width, 0);
            g.DrawString(scoreText, font, Brushes.White, location);
        }
    }
}
