using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

namespace test
{
    public partial class KillFlyGame : Form
    {
        public FlyModel Model { get; set; }
        public FlyController Controller { get; set; }
        private Image FlyImage;
        private Bitmap BackgroundBitmap;

        public KillFlyGame()
        {
            InitializeComponent();
            Model = new FlyModel();
            Controller = new FlyController(Model, this);
            FlyImage = Image.FromFile("Resources/fly.png");
            BackgroundBitmap = new Bitmap(Image.FromFile("Resources/background.jpg")); 
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
            private KillFlyGame view;
            private Random random = new Random();
            private Timer gameTimer;
            private Timer ballTimer;
            private DateTime startTime;

            public FlyController(FlyModel fly, KillFlyGame view)
            {
                this.fly = fly;
                this.view = view;
                
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
                            fly.IsGrowing = false;
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

                    view.Invalidate();
                };
                ballTimer.Start();

                view.MouseClick += (s, e) =>
                {
                    var flyRectangle = new Rectangle(new Point(fly.Center.X - fly.Size / 2, fly.Center.Y - fly.Size / 2), new Size(fly.Size, fly.Size));
                    if (flyRectangle.Contains(e.Location))
                    {
                        fly.Score += CalculateScore();
                        fly.Size = 1;
                        fly.Center = GetRandomPosition();
                        view.Invalidate();
                    }
                };
            }

            private Point GetRandomPosition()
            {
                return new Point(
                    random.Next(50, view.ClientSize.Width - 50),
                    random.Next(50, view.ClientSize.Height - 50));
            }

            private int CalculateScore()
            {

                var maxScore = 300;
                var minScore = 50;
                var scoreDecreaseRate = fly.Size / (double)fly.MaxSize; 
                var score = maxScore - (int)(300 * scoreDecreaseRate);
                return Math.Max(score, minScore);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DoubleBuffered = true;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawImage(BackgroundBitmap, 0, 0);
            e.Graphics.DrawImage(FlyImage, new Rectangle(new Point(Model.Center.X - Model.Size / 2, Model.Center.Y - Model.Size / 2), new Size(Model.Size, Model.Size)));
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile("Fonts/BetterVCR.ttf");
            var font = new Font(pfc.Families[0], 16, FontStyle.Bold);

            var timeText = Model.TimeLeft.ToString(@"ss");
            var textSize = e.Graphics.MeasureString(timeText, font);
            var location = new PointF((ClientSize.Width - textSize.Width) / 2, ClientSize.Height - textSize.Height);
            e.Graphics.DrawString(timeText, font, Brushes.White, location);

            var scoreText = "Очки: " + Model.Score.ToString();
            textSize = e.Graphics.MeasureString(scoreText, font);
            location = new PointF(ClientSize.Width - textSize.Width, 0);
            e.Graphics.DrawString(scoreText, font, Brushes.White, location);
        }
    }
}
