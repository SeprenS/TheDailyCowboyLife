using KillFlyGame.Model;
using System;
using System.Drawing;
using System.Windows.Forms;
using TheDailyCowboyLife;

namespace KillFlyGame.Controller
{
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
                        mainForm.KillFlyGame.FlyNoice.Stop();
                        fly.Score -= 150;
                        if (fly.Score < 0)
                            fly.Score = 0;
                    }
                }
                else
                {
                    fly.Size = 1;
                    fly.Center = GetRandomPosition();
                    mainForm.KillFlyGame.FlyNoice.Play();
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
                mainForm.KillFlyGame.FlyNoice.Stop();
                mainForm.KillFlyGame.FlyNoice.Play();
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
}
