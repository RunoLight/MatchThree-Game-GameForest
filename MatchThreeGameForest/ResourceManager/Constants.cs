using Microsoft.Xna.Framework;
using System;

namespace MatchThreeGameForest.ResourceManager
{
    public static class Constants
    {
        public static String ResourcesRoot = "Content";

        public static String PathFont = "Fonts/Font";
        public static String PathCellBackground = "Sprites/CellBackground";
        public static String PathGameOverScreen = "Sprites/Gameover";
        public static String PathMenuButton = "Sprites/MenuButton";
        public static String PathPlayButton = "Sprites/PlayButton";
        public static String PathBlankTexture = "Sprites/BlankTexture";
        public static String PathBlankCell = "Sprites/BlankCell";
        public static String PathDestroyer = "Sprites/Destroyer";

        public static String PathElements = "Sprites/Elements/";
        public static String TypeLineV = "LineV";
        public static String TypeLineH = "LineH";
        public static String TypeBomb = "Bomb";


        public static Color ButtonColor = Color.White;
        public static Color ButtonColorHighlighted = Color.LightBlue;
        public static Color ButtonColorClicked = Color.DodgerBlue;

        public static float MenuTransitionTime = 0.5F;
        public static String LoadingScreenMessage = "Oh, hi there!";
        public static Color TextColor = Color.Black;

        public static float GameRoundTime = 30F;

        public static int GridSize = 8;

    }
}
