using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DungeonCrawler
{
    class InputManager
    {
        Dictionary<Keys, GameAction> mapping;

        public InputManager()
        {
            mapping = new Dictionary<Keys, GameAction>();
        }

        public void addMapping(Keys key, GameAction action)
        {
            mapping.Add(key, action);
        }

        public void execute(Keys key)
        {
            mapping[key].Invoke();
        }
    }
}
