using UnityEngine;

namespace MoreMountains.Tools
{
    /// <summary>
    /// An event used to broadcast slider events from a MMDebugMenu
    /// </summary>
    public struct MMDebugMenuSliderEvent
    {
        public delegate void Delegate(string sliderEventName, float value);
        static private event Delegate OnEvent;

        static public void Register(Delegate callback)
        {
            OnEvent += callback;
        }

        static public void Unregister(Delegate callback)
        {
            OnEvent -= callback;
        }

        static public void Trigger(string sliderEventName, float value)
        {
            OnEvent?.Invoke(sliderEventName, value);
        }
    }
}