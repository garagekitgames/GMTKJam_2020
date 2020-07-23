using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A class used to bind a button to a MMDebugMenu
    /// </summary>
    public class MMDebugMenuItemButton : MonoBehaviour
    {
        [Header("Bindings")]
        /// the button's text comp
        public Text ButtonText;
        /// the button's background image
        public Image ButtonBg;
        /// the name of the event bound to this button
        public string ButtonEventName = "Button";

        /// <summary>
        /// Triggers a button event using the button's event name
        /// </summary>
        public virtual void TriggerButtonEvent()
        {
            MMDebugMenuButtonEvent.Trigger(ButtonEventName);
        }
    }
}
