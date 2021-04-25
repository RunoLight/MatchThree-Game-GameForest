using MatchThreeGameForest.GameLogic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static MatchThreeGameForest.ResourceManager.Constants;

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

            Font = content.Load<SpriteFont>(PathFont);

            Cell = content.Load<Texture2D>(PathCellBackground);
            GameOverScreen = content.Load<Texture2D>(PathGameOverScreen);
            MenuButton = content.Load<Texture2D>(PathMenuButton);
            PlayButton = content.Load<Texture2D>(PathPlayButton);
            BlankTexture = content.Load<Texture2D>(PathBlankTexture);
            Destroyer = content.Load<Texture2D>(PathDestroyer);

            elementsDict.Add(ShapeType.Empty, Enumerable.Repeat(content.Load<Texture2D>(PathBlankCell), 4).ToArray());

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

        private static Texture2D[] buildTexturesByType(ShapeType type)
        {
            return new Texture2D[4]
            {
                content.Load<Texture2D>(PathElements + type),
                content.Load<Texture2D>(PathElements + type + TypeLineV),
                content.Load<Texture2D>(PathElements + type + TypeLineH),
                content.Load<Texture2D>(PathElements + type + TypeBomb)
            };
        }
        #endregion
    }
}
