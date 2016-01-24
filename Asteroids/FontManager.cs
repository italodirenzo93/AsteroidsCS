using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    public static class FontManager
    {
        public static SpriteFont ArDestine16;
        public static SpriteFont ArDestine22;

        public static void LoadFonts(ContentManager content)
        {
            ArDestine16 = content.Load<SpriteFont>("Fonts/ARDESTINE_16");
            ArDestine22 = content.Load<SpriteFont>("Fonts/ARDESTINE_22");
        }
    }
}
