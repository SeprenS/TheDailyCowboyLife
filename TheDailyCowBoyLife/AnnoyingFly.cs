using System;
using System.Drawing;
using System.Windows.Forms;

namespace test
{
    public partial class BallTrackingGame : Form
    {
        public BallModel Model { get; set; }
        public BallController Controller { get; set; }

        public BallTrackingGame()
        {
            InitializeComponent();
            Model = new BallModel();
            Controller = new BallController(Model, this);
        }
        // Модель
        public class BallModel
        {
            public Point Center { get; set; }
            public int Size { get; set; }
            public bool IsGrowing { get; set; }
            public TimeSpan TimeLeft { get; set; } // новое свойство для отслеживания оставшегося времени
            public int Score { get; set; } // новое свойство для отслеживания очков
            public int MaxSize = 50;
            public int MinSize = 1;
        }

        // Контроллер
        public class BallController
        {
            private BallModel ball;
            private BallTrackingGame view;
            private Random random = new Random();
            private Timer gameTimer;
            private Timer ballTimer;
            private DateTime startTime;

            public BallController(BallModel ball, BallTrackingGame view)
            {
                this.ball = ball;
                this.view = view;

                ball.Center = GetRandomPosition();
                ball.Size = 1;
                ball.IsGrowing = true;
                ball.TimeLeft = TimeSpan.FromMinutes(1); // устанавливаем начальное время
                ball.Score = 0; // начальное количество очков

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
                    if (ball.IsGrowing)
                    {
                        ball.Size++;
                        if (ball.Size >= 50)
                            ball.IsGrowing = false;
                    }
                    else
                    {
                        ball.Size = 1;
                        ball.Center = GetRandomPosition();
                        ball.IsGrowing = true;
                    }

                    var elapsed = DateTime.Now - startTime;
                    ballTimer.Interval = (int)(25 * (1 + elapsed.TotalSeconds / 60));

                    // обновляем оставшееся время
                    ball.TimeLeft = TimeSpan.FromMinutes(1) - elapsed;
                    if (ball.TimeLeft < TimeSpan.Zero)
                        ball.TimeLeft = TimeSpan.Zero;

                    view.Invalidate();
                };
                ballTimer.Start();

                view.MouseClick += (s, e) =>
                {
                    var ballRectangle = new Rectangle(new Point(ball.Center.X - ball.Size / 2, ball.Center.Y - ball.Size / 2), new Size(ball.Size, ball.Size));
                    if (ballRectangle.Contains(e.Location))
                    {
                        ball.Score += CalculateScore();
                        ball.Size = 1;
                        ball.Center = GetRandomPosition();
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

                int maxScore = 300;
                int minScore = 50;
                double scoreDecreaseRate = ball.Size / 50.0; // 50 - максимальный размер шарика
                int score = maxScore - (int)(300 * scoreDecreaseRate);
                return Math.Max(score, minScore); // гарантируем, что очки не опускаются ниже минимального значения
            }




        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(new Point(Model.Center.X - Model.Size / 2, Model.Center.Y - Model.Size / 2), new Size(Model.Size, Model.Size)));

            // рисуем оставшееся время по центру снизу
            string timeText = Model.TimeLeft.ToString(@"ss");
            Font font = new Font("Times New Roman", 16);
            SizeF textSize = e.Graphics.MeasureString(timeText, font);
            PointF location = new PointF((ClientSize.Width - textSize.Width) / 2, ClientSize.Height - textSize.Height);
            e.Graphics.DrawString(timeText, font, Brushes.Black, location);

            // рисуем количество очков справа сверху
            string scoreText = "Очки: " + Model.Score.ToString();
            textSize = e.Graphics.MeasureString(scoreText, font);
            location = new PointF(ClientSize.Width - textSize.Width, 0);
            e.Graphics.DrawString(scoreText, font, Brushes.Black, location);
        }
    }
}
