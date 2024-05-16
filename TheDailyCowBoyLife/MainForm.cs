using System;
using System.Drawing;
using System.Windows.Forms;
using KillFlyGame;


namespace TheDailyCowboyLife
{
    public partial class MainForm : Form
    {
        public KillFlyGame.KillFlyGame KillFlyGame { get; }

        public MainForm()
        {
            InitializeComponent();
            KillFlyGame = new KillFlyGame.KillFlyGame(this);
            this.MouseClick += KillFlyGame.Controller.HandleMouseClick;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
            base.OnPaint(e);
            KillFlyGame.Draw(e.Graphics);
        }
    }

}
