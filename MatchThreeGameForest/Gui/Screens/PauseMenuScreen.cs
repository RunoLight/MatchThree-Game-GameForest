﻿using MatchThreeGameForest.GameStateManagement;
using Microsoft.Xna.Framework;

namespace MatchThreeGameForest.Gui.Screens
{
    class PauseMenuScreen : MenuScreen
    {
        public PauseMenuScreen() : base()
        {
            MenuEntry resumeGameMenuEntry = new MenuEntry(TextureRenderer.TextureManager.Diamond, new Point(280, 200));
            //MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
            //resumeGameMenuEntry.Selected += OnCancel;
            //quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            //MenuEntries.Add(resumeGameMenuEntry);
            //MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }
    }
}