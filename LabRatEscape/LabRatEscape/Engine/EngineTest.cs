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

namespace LabRatEscape
{
    public class EngineTest : Engine
    {
        public EngineTest(Game1 game) : base(game)
        {
        }

        public override void Update()
        {
            // update each branch of the game in the appropriate order
            CheckCollisions();
            game.getPlayer().Update();
            //game.enemyManager.Update();
            //game.itemManager.Update();
            //game.levelManager.Update();
        }

        public override void CheckCollisions()
        {
            base.CheckCollisions();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawScreen(SpriteBatch spriteBatch, float opacity)
        {
            base.DrawScreen(spriteBatch, opacity);
        }
    }
}
