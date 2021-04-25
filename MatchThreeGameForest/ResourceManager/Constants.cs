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
        public static String LoadingScreenMessage = "Did you know? Unity is good engine too";
        public static Color TextColor = Color.Black;

        public static float GameRoundTime = 30F;

        public static int GridSize = 8;
        public static Point GridOffset = new Point(10, 10);

        public static float CellFadeSpeed = 4f;
        public static float CellFallSpeed = 400f;

        public static int CellSwapSpeed = 200;
        public static int CellSwapBackSpeed = 250;

        public static Color CellColor = Color.White;
        public static Color CellHoverColor = Color.Chocolate;
        public static Color CellPressedColor = Color.Orange;
        public static Color CellSelectedColor = Color.Red;

        public static float DISTANCE_TOLERANCE = 0.5f;
        public static float OPACITY_LERP_SPEED = 4.5f;
        public static float MOVING_LERP_SPEED = 10f;


    }
}
