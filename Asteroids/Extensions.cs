using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public static class Extensions
    {
        public static void DrawShadowedString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}
