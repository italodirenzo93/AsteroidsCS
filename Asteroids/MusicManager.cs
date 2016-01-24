using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Asteroids
{
    public static class MusicManager
    {
        private static Dictionary<string, Song> music = new Dictionary<string, Song>();

        public static void LoadMusic(ContentManager content)
        {

        }

        public static Song GetMusic(string name)
        {
            return music[name];
        }
    }
}
