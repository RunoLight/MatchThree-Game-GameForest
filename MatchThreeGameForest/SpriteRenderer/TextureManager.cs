using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MatchThreeGameForest.TextureRenderer
{
    static class TextureManager
    {
        public static Texture2D Cell { get; private set; }

        public static Texture2D Diamond { get; private set; }
        public static Texture2D Ellipse { get; private set; }
        public static Texture2D Hexagon { get; private set; }
        public static Texture2D Pentagon { get; private set; }
        public static Texture2D Star { get; private set; }

        public static void Init(ContentManager content)
        {
            Cell = content.Load<Texture2D>("Sprites/CellBackground");

            Diamond = content.Load<Texture2D>("Sprites/Elements/Diamond");
            Ellipse = content.Load<Texture2D>("Sprites/Elements/Ellipse");
            Hexagon = content.Load<Texture2D>("Sprites/Elements/Hexagon");
            Pentagon = content.Load<Texture2D>("Sprites/Elements/Pentagon");
            Star = content.Load<Texture2D>("Sprites/Elements/Star");
        }

        public static void Deinit()
        {
            List<Texture2D> list = new List<Texture2D>() { Cell, Diamond, Ellipse, Hexagon, Pentagon, Star };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Dispose();
                list[i] = null;
            }
        }

    }
}
