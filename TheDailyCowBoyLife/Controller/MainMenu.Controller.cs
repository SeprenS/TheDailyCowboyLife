using System;
using System.Drawing;
using System.Windows.Forms;
using TheDailyCowboyLife;

namespace Menu
{
    public class MainMenuController
    {
        private MainForm mainForm;
        private MainMenu mainMenu;
        private System.Windows.Forms.MainMenu menuView;

        public MainMenuController(MainForm form, MainMenu menu)
        {
            this.mainForm = form;
            this.mainMenu = menu;

            this.mainForm.MouseClick += HandleMouseClick;
        }

        public MainMenuController(MainForm mainForm, System.Windows.Forms.MainMenu menuView)
        {
            this.mainForm = mainForm;
            this.menuView = menuView;
        }

        public void HandleMouseClick(object sender, MouseEventArgs e)
        {
            if (mainMenu.StartButtonRectangle.Contains(e.Location))
            {
                this.mainForm.KillFlyGame = new KillFlyGame.KillFlyGame(this.mainForm);
                this.mainForm.MouseClick += this.mainForm.KillFlyGame.Controller.HandleMouseClick;
                this.mainForm.Invalidate();
            }
            else if (mainMenu.ExitButtonRectangle.Contains(e.Location))
                Application.Exit();
        }
    }
}
