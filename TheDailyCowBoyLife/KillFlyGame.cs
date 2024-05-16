using System;
using System.Drawing;
using System.Windows.Media;
using System.Media;
using Brushes = System.Drawing.Brushes;
using System.Drawing.Text;
using TheDailyCowboyLife;
using KillFlyGame.Model;
using KillFlyGame.Controller;

namespace KillFlyGame
{
    public class KillFlyGame
    {
        public FlyModel Model { get; set; }
        public FlyController Controller { get; set; }
        private Image FlyImage;
        private Bitmap BackgroundBitmap;
        private static MainForm mainForm;
        public SoundPlayer HitSound;
        public SoundPlayer MissSound;
        public MediaPlayer FlyNoice;


        public KillFlyGame(MainForm form)
        {
            mainForm = form;
            Model = new FlyModel(); 
            Controller = new FlyController(Model, form);
            FlyNoice = new MediaPlayer();
            FlyNoice.Open(new Uri("Resources/Sounds/flynoice.mp3", UriKind.Relative));
            FlyNoice.Volume = 0.4; FlyNoice.Play();
            MissSound = new SoundPlayer("Resources/Sounds/miss.wav");
            HitSound = new SoundPlayer("Resources/Sounds/hit.wav");
            FlyImage = Image.FromFile("Resources/Images/fly.png");
            BackgroundBitmap = new Bitmap(Image.FromFile("Resources/Images/background.jpg"));
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
