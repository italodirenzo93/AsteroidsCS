using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    public static class SoundEffectManager
    {
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public static void LoadSounds(ContentManager content)
        {
            sounds.Add("shipLaser", content.Load<SoundEffect>("Audio/laser1"));
        }

        public static SoundEffect GetSound(string name)
        {
            return sounds[name];
        }
    }
}
