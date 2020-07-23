using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A class used to bind a slider to a MMDebugMenu
    /// </summary>
    public class MMDebugMenuItemSlider : MonoBehaviour
    {
        /// the possible modes this slider can operate on
        public enum Modes { Float, Int }

        [Header("Bindings")]
        /// the selected mode for this slider
        public Modes Mode = Modes.Float;
        /// the Slider to use to change the value
        public Slider TargetSlider;
        /// the text comp used to display the slider's name
        public Text SliderText;
        /// the text comp used to display the slider's value
        public Text SliderValueText;
        /// the target knob
        public Image SliderKnob;
        /// the line behind the knob
        public Image SliderLine;
        /// the value to remap the slider's 0 to
        public float RemapZero = 0f;
        /// the value to remap the slider's 1 to
        public float RemapOne = 1f;
        /// the name of the event bound to this slider
        public string SliderEventName = "Checkbox";

        /// the current slider value
        [MMReadOnly]
        public float SliderValue;
        /// the current slider int value
        [MMReadOnly]
        public int SliderValueInt;
        
        /// <summary>
        /// On Awake we start listening for slider changes 
        /// </summary>
        protected virtual void Awake()
        {
            TargetSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        /// <summary>
        /// Invoked when the slider value changes
        /// </summary>
        public void ValueChangeCheck()
        {
            bool valueChanged = true;

            SliderValue = MMMaths.Remap(TargetSlider.value, 0f, 1f, RemapZero, RemapOne);

            if (Mode == Modes.Int)
            {
                SliderValue = Mathf.Round(SliderValue);
                if (SliderValue == SliderValueInt)
                {
                    valueChanged = false;
                }
                SliderValueInt = (int)SliderValue;
            }

            if (valueChanged)
            {
                SliderValueText.text = (Mode == Modes.Int) ? SliderValue.ToString() : SliderValue.ToString("F3");
                TriggerSliderEvent(SliderValue);
            }
        }

        /// <summary>
        /// Triggers a slider event
        /// </summary>
        /// <param name="value"></param>
        protected virtual void TriggerSliderEvent(float value)
        {
            MMDebugMenuSliderEvent.Trigger(SliderEventName, value);
        }
    }
}