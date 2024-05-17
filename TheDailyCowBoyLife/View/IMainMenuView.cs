using System;

namespace IMainMenuView
{ 
    public interface IMainMenuView
    {
        event EventHandler StartGameClicked;
        event EventHandler ExitClicked;

        void ShowView();
        void HideView();
    }
}
