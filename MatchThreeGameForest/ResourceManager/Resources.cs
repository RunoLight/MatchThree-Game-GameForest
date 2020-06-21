using MatchThreeGameForest.GameLogic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MatchThreeGameForest.ResourceManager
{
    static class Resources
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D Cell { get; private set; }
        public static Texture2D GameOverScreen { get; private set; }
        public static Texture2D MenuButton { get; private set; }
        public static Texture2D PlayButton { get; private set; }
        public static Texture2D BlankTexture { get; private set; }
        public static Texture2D Destroyer { get; private set; }

        public static Texture2D GetTexture(ShapeType shape, Bonus bonus)
        {
            return elementsDict[shape][(int)bonus];
        }

        public static void Init(ContentManager Content)
        {
            content = Content;

            Font = content.Load<SpriteFont>("Fonts/Font");

            Cell = content.Load<Texture2D>("Sprites/CellBackground");
            GameOverScreen = content.Load<Texture2D>("Sprites/Gameover");
            MenuButton = content.Load<Texture2D>("Sprites/MenuButton");
            PlayButton = content.Load<Texture2D>("Sprites/PlayButton");
            BlankTexture = content.Load<Texture2D>("Sprites/BlankTexture");
            Destroyer = content.Load<Texture2D>("Sprites/Destroyer");

            elementsDict.Add(ShapeType.Empty, Enumerable.Repeat(content.Load<Texture2D>("Sprites/BlankCell"), 4).ToArray());

            MemberInfo[] memberInfos = typeof(ShapeType).GetMembers(BindingFlags.Public | BindingFlags.Static);
            for (int i = 1; i < memberInfos.Length; i++)
            {
                ShapeType type = (ShapeType)Enum.Parse(typeof(ShapeType), memberInfos[i].Name);
                elementsDict.Add(type, buildTexturesByType(type));
            }
        }

        #region Inner-space logic
        private static ContentManager content;

        private static Dictionary<ShapeType, Texture2D[]> elementsDict = new Dictionary<ShapeType, Texture2D[]>();

        private static string element = "Sprites/Elements/";
        private static string lineH = "LineH";
        private static string lineV = "LineV";
        private static string bomb = "Bomb";
        private static Texture2D[] buildTexturesByType(ShapeType type)
        {
            return new Texture2D[4]
            {
                content.Load<Texture2D>(element + type),
                content.Load<Texture2D>(element + type + lineV),
                content.Load<Texture2D>(element + type + lineH),
                content.Load<Texture2D>(element + type + bomb)
            };
        }
        #endregion
    }
}
