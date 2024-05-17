using System.Windows.Forms;
using TheDailyCowboyLife;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Media;
using System;

namespace Menu
{
    public class MainMenu
    {
        private MainForm mainForm;
        public Bitmap StartButtonBitmap;
        public Bitmap ExitButtonBitmap;
        public MediaPlayer WesternSong;
        private Bitmap BackgroundBitmap;
        public Rectangle StartButtonRectangle;
        public Rectangle ExitButtonRectangle;

        public MainMenu(MainForm form)
        {
            this.mainForm = form;
            WesternSong = new MediaPlayer();
            WesternSong.Open(new Uri("Resources/Sounds/mainmenusong.mp3", UriKind.Relative));
            WesternSong.Volume = 0.4; WesternSong.Play();
            BackgroundBitmap = new Bitmap(Image.FromFile("Resources/Images/MainMenu/MainMenuBackground.jpg"));
            StartButtonBitmap = new Bitmap(Image.FromFile("Resources/Images/MainMenu/startbutton.png"));
            ExitButtonBitmap = new Bitmap(Image.FromFile("Resources/Images/MainMenu/exitbutton.png"));
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            this.StartButtonRectangle = new Rectangle(this.mainForm.ClientSize.Width / 2 - 50, this.mainForm.ClientSize.Height / 2, 150, 150);
            this.ExitButtonRectangle = new Rectangle(this.mainForm.ClientSize.Width / 2 - 50, this.mainForm.ClientSize.Height / 2 + 200, 150, 150);
        }

        public void Draw(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(BackgroundBitmap, 0, 0);
            g.DrawImage(StartButtonBitmap, StartButtonRectangle);
            g.DrawImage(ExitButtonBitmap, ExitButtonRectangle);
        }
    }
}
