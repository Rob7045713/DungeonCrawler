// Copyright 12.13.2010
// Rob Argue

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LabRatEscape
{
    public class SoundManager
    {
        #region Variables
        public static Dictionary<string, SoundEffect> effects = new Dictionary<string,SoundEffect>();
        public static Dictionary<string, Song> songs = new Dictionary<string,Song>();
        #endregion

        #region Methods

        public static void PlayEffect(string effect)
        {
            if(effects.ContainsKey(effect))
            {
                effects[effect].Play();
            }
        }

        public static void PlaySong(string song)
        {
            MediaPlayer.IsRepeating = true;
            if (songs.ContainsKey(song))
            {
                MediaPlayer.Play(songs[song]);
            }
        }
        #endregion
    }
}
