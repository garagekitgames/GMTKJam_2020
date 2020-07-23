using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A class used to bind a checkbox to a MMDebugMenu
    /// </summary>
    public class MMDebugMenuItemCheckbox : MonoBehaviour
    {
        [Header("Bindings")]
        /// the switch used to display the checkbox
        public MMDebugMenuSwitch Switch;
        /// the text used to display the checkbox's text
        public Text SwitchText;
        /// the name of the checkbox event
        public string CheckboxEventName = "Checkbox";

        /// <summary>
        /// Triggers an event when the checkbox gets pressed
        /// </summary>
        public virtual void TriggerCheckboxEvent()
        {
            MMDebugMenuCheckboxEvent.Trigger(CheckboxEventName, Switch.SwitchState);
        }

        /// <summary>
        /// Triggers an event when the checkbox gets checked and becomes true
        /// </summary>
        public virtual void TriggerCheckboxEventTrue()
        {
            MMDebugMenuCheckboxEvent.Trigger(CheckboxEventName, true);
        }

        /// <summary>
        /// Triggers an event when the checkbox gets unchecked and becomes false
        /// </summary>
        public virtual void TriggerCheckboxEventFalse()
        {
            MMDebugMenuCheckboxEvent.Trigger(CheckboxEventName, false);
        }
    }
}
