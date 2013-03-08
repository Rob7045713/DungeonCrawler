// Shannon Duvall
// Rob Argue
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LabRatEscape
{
    // This class contains everything needed to call a Sprite's 
    // method using reflection.
    public class GameAction
    {
        Object actor;
        Type type;
        MethodInfo method;
        object[] parameters;

        public GameAction(Type type, MethodInfo method, object[] param)
        {
            this.type = type;
            this.method = method;
            this.parameters = param;
        }

        public GameAction(Object actor, MethodInfo method, object[] param)
        {
            this.actor = actor;
            this.method = method;
            this.parameters = param;
        }

        public void Invoke()
        {
            if (actor != null)
            {
                method.Invoke(actor, parameters);
            }
            if (type != null)
            {
                method.Invoke(null, parameters);
            }
        }

        /// <summary>
        /// Changes the parameters and then invokes the method
        /// </summary>
        /// <param name="parameters">New parameters to use for invoking the method</param>
        public void Invoke(object[] parameters)
        {
            this.parameters = parameters;
            this.Invoke();
        }

        /// <summary>
        /// Changes the actor and then invokes the method
        /// </summary>
        /// <param name="actor">New actor to invoke the method on</param>
        public void Invoke(Object actor)
        {
            this.actor = actor;
            this.Invoke();
        }

        /// <summary>
        /// Changes the actor and parameters, and then invokes the method
        /// </summary>
        /// <param name="actor">New actor to invoke the method on</param>
        /// <param name="parameters">New parameters to use for invoking the method</param>
        public void Invoke(Object actor, object[] parameters)
        {
            this.actor = actor;
            this.parameters = parameters;
            this.Invoke();
        }
    }

}