using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class BallTrackingGame : Form
    {
        private const int MaxBallSize = 50;
        private const int MinBallSize = 1;
        private int ballSize = MinBallSize;
        private Point ballCenter;
        private Random random = new Random();
        private Timer gameTimer;
        private Timer ballTimer;
        private bool isGrowing = true;
        private DateTime startTime;

        public BallTrackingGame()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ballCenter = GetRandomPosition();
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
                if (isGrowing)
                {
                    ballSize++;
                    if (ballSize >= MaxBallSize)
                    {
                        isGrowing = false;
                    }
                }
                else
                {
                    ballSize = MinBallSize;
                    ballCenter = GetRandomPosition();
                    isGrowing = true;
                }

                var elapsed = DateTime.Now - startTime;
                ballTimer.Interval = (int)(25  * (1 + elapsed.TotalSeconds / 60));

                Invalidate();
            };
            ballTimer.Start();

            MouseClick += (s, e) =>
            {
                var ballRectangle = new Rectangle(new Point(ballCenter.X - ballSize / 2, ballCenter.Y - ballSize / 2), new Size(ballSize, ballSize));
                if (ballRectangle.Contains(e.Location))
                {
                    ballSize = MinBallSize;
                    ballCenter = GetRandomPosition();
                    Invalidate();
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(new Point(ballCenter.X - ballSize / 2, ballCenter.Y - ballSize / 2), new Size(ballSize, ballSize)));
        }

        private Point GetRandomPosition()
        {
            return new Point(
                random.Next(MaxBallSize, ClientSize.Width - MaxBallSize),
                random.Next(MaxBallSize, ClientSize.Height - MaxBallSize));
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }
    }
}
