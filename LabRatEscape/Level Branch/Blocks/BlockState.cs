// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Interface for a state which can update and draw a sprite
    /// </summary>
    public interface BlockState : State
    {
        void Update(Sprite sprite, float elapsedTime);
        void Draw(Sprite sprite, SpriteBatch batch);
        void ApplyEffects(Block block, Sprite sprite);
    }
}
