using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DungeonCrawler
{
    class GameAction
    {
        MethodInfo method;
        Object target;
        object[] parameters;

        public GameAction(Object target, MethodInfo method, object[] parameters)
        {
            this.target = target;
            this.method = method;
            this.parameters = parameters;
        }

        public void Invoke()
        {
            method.Invoke(target, parameters);
        }
    }
}
