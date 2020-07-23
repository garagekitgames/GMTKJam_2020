using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
    /// <summary>
    /// An event fired when a button gets pressed in a MMDebugMenu
    /// </summary>
    [Serializable]
    public class MMDButtonPressedEvent : UnityEvent
    {
    }

    /// <summary>
    /// A class used to listen to button events from a MMDebugMenu
    /// </summary>
    public class MMDebugMenuButtonEventListener : MonoBehaviour
    {
        [Header("Event")]
        /// the name of the event to listen to
        public string ButtonEventName = "Button";
        /// an event to fire when the event is heard
        public MMDButtonPressedEvent MMDEvent;

        /// <summary>
        /// When we get a menu button event, we invoke
        /// </summary>
        /// <param name="buttonEventName"></param>
        protected virtual void OnMMDebugMenuButtonEvent(string buttonEventName)
        {
            if (buttonEventName == ButtonEventName)
            {
                if (MMDEvent != null)
                {
                    MMDEvent.Invoke();
                }
            }
        }

        /// <summary>
        /// Starts listening for events
        /// </summary>
        public virtual void OnEnable()
        {
            MMDebugMenuButtonEvent.Register(OnMMDebugMenuButtonEvent);
        }

        /// <summary>
        /// Stops listening for events
        /// </summary>
        public virtual void OnDisable()
        {
            MMDebugMenuButtonEvent.Unregister(OnMMDebugMenuButtonEvent);
        }
    }
}
