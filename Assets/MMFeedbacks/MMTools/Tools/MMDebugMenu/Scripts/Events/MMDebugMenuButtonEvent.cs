using UnityEngine;

namespace MoreMountains.Tools
{
    /// <summary>
    /// An event used to broadcast button events from a MMDebugMenu
    /// </summary>
    public struct MMDebugMenuButtonEvent
    {
        public delegate void Delegate(string buttonEventName);
        static private event Delegate OnEvent;

        static public void Register(Delegate callback)
        {
            OnEvent += callback;
        }

        static public void Unregister(Delegate callback)
        {
            OnEvent -= callback;
        }

        static public void Trigger(string buttonEventName)
        {
            OnEvent?.Invoke(buttonEventName);
        }
    }
}