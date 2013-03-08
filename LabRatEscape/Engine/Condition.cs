// Copyright 12.13.2010
// Rob Argue

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Uses reflection to check if a method with a return type of bool is true
    /// </summary>
    public class Condition
    {
        #region Variables
        Object actor;
        MethodInfo method;
        object[] parameters;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for condition
        /// </summary>
        public Condition()
        {
        }

        /// <summary>
        /// Constructor for a new condition
        /// </summary>
        /// <param name="method">Method with a return type of bool to call</param>
        /// <param name="parameters">Parameters for calling the method</param>
        public Condition(MethodInfo method, object[] parameters) : this(null, method, parameters)
        {}

        /// <summary>
        /// Constructor for a new condition
        /// </summary>
        /// <param name="actor">Object who's method is being called</param>
        /// <param name="method">Method with a return type of bool to call</param>
        /// <param name="parameters">Parameters for calling the method</param>
        public Condition(Object actor, MethodInfo method, object[] parameters)
        {
            this.actor = actor;
            this.method = method;
            this.parameters = parameters;
        }
        #endregion

        #region Methods

        public static bool DefaultCondition()
        {
            return true;
        }

        /// <summary>
        /// Checks if the condition has been met
        /// </summary>
        /// <returns>True if the condition has been met</returns>
        public bool CheckCondition()
        {
            if (method != null)
            {
                object output = method.Invoke(actor, parameters);
                if (output is bool)
                {
                    return (bool)output;
                }
            }
            return false;

        }

        /// <summary>
        /// Checks if the method has been met, using new parameters
        /// </summary>
        /// <param name="parameters">New parameters to use</param>
        /// <returns>True if the condition has been met</returns>
        public bool CheckCondition(object[] parameters)
        {
            this.parameters = parameters;
            return CheckCondition();
        }

        /// <summary>
        /// Checks if the method has been met, using a new actor
        /// </summary>
        /// <param name="actor">New actor to use</param>
        /// <returns>True if the condition has been met</returns>
        public bool CheckCondition(Object actor)
        {
            this.actor = actor;
            return CheckCondition();
        }

        /// <summary>
        /// Checks if the method has been met, using a new actor and new parameters
        /// </summary>
        /// <param name="actor">New actor to use</param>
        /// <param name="parameters">New parameters to use</param>
        /// <returns>True if the condition has been met</returns>
        public bool CheckCondition(Object actor, object[] parameters)
        {
            this.actor = actor;
            this.parameters = parameters;
            return CheckCondition();
        }
        #endregion
    }

}