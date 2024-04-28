using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace test
{
    // Модель
    public class BallModel
    {
        public Point Center { get; set; }
        public int Size { get; set; }
        public bool IsGrowing { get; set; }
        public TimeSpan TimeLeft { get; set; } // новое свойство для отслеживания оставшегося времени
    }

    // Контроллер
    public class BallController
    {
        private BallModel model;
        private BallTrackingGame view;
        private Random random = new Random();
        private Timer gameTimer;
        private Timer ballTimer;
        private DateTime startTime;

        public BallController(BallModel model, BallTrackingGame view)
        {
            this.model = model;
            this.view = view;

            model.Center = GetRandomPosition();
            model.Size = 1;
            model.IsGrowing = true;
            model.TimeLeft = TimeSpan.FromMinutes(1); // устанавливаем начальное время

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
                if (model.IsGrowing)
                {
                    model.Size++;
                    if (model.Size >= 50)
                    {
                        model.IsGrowing = false;
                    }
                }
                else
                {
                    model.Size = 1;
                    model.Center = GetRandomPosition();
                    model.IsGrowing = true;
                }

                var elapsed = DateTime.Now - startTime;
                ballTimer.Interval = (int)(25 * (1 + elapsed.TotalSeconds / 60));

                // обновляем оставшееся время
                model.TimeLeft = TimeSpan.FromMinutes(1) - elapsed;
                if (model.TimeLeft < TimeSpan.Zero)
                {
                    model.TimeLeft = TimeSpan.Zero;
                }

                view.Invalidate();
            };
            ballTimer.Start();

            view.MouseClick += (s, e) =>
            {
                var ballRectangle = new Rectangle(new Point(model.Center.X - model.Size / 2, model.Center.Y - model.Size / 2), new Size(model.Size, model.Size));
                if (ballRectangle.Contains(e.Location))
                {
                    model.Size = 1;
                    model.Center = GetRandomPosition();
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
    }


    public partial class BallTrackingGame : Form
    {
        public BallModel Model { get; set; }
        public BallController Controller { get; set; }

        private PrivateFontCollection privateFonts = new PrivateFontCollection();

        public BallTrackingGame()
        {
            InitializeComponent();
            Model = new BallModel();
            Controller = new BallController(Model, this);
            privateFonts.AddFontFile("Fonts/BetterVCR.ttf");

            // Проверьте, был ли шрифт успешно загружен
            if (privateFonts.Families.Length > 0)
            {
                Console.WriteLine("Шрифт успешно загружен!");
            }
            else
            {
                Console.WriteLine("Не удалось загрузить шрифт.");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(new Point(Model.Center.X - Model.Size / 2, Model.Center.Y - Model.Size / 2), new Size(Model.Size, Model.Size)));

            // Используйте загруженный шрифт
            var font = new Font(privateFonts.Families[0], 16);
            string timeText = Model.TimeLeft.ToString(@"ss");
            SizeF textSize = e.Graphics.MeasureString(timeText, font);
            PointF location = new PointF((ClientSize.Width - textSize.Width) / 2, ClientSize.Height - textSize.Height);
            e.Graphics.DrawString(timeText, font, Brushes.Black, location);
        }

    }
}
