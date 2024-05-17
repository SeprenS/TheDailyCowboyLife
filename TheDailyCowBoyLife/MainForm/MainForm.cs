using System;
using System.Drawing;
using System.Windows.Forms;
using KillFlyGame;
using Menu;

namespace TheDailyCowboyLife
{
    public partial class MainForm : Form
    {
        public KillFlyGame.KillFlyGame KillFlyGame { get; set; }
        private Menu.MainMenu menuView;
        private Menu.MainMenuController menuController;

        public MainForm()
        {
            InitializeComponent();
            menuView = new Menu.MainMenu(this);
            menuController = new Menu.MainMenuController(this, menuView);
            this.MouseClick += menuController.HandleMouseClick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (KillFlyGame != null)
            {
                this.DoubleBuffered = true;
                base.OnPaint(e);
                KillFlyGame.Draw(e.Graphics);
            }
            else
            {
                menuView.Draw(e.Graphics);
            }
        }
    }
}
