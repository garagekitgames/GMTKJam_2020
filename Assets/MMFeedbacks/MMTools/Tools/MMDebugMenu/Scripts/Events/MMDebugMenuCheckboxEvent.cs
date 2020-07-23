using UnityEngine;

namespace MoreMountains.Tools
{
    /// <summary>
    /// An event used to broadcast checkbox events from a MMDebugMenu
    /// </summary>
    public struct MMDebugMenuCheckboxEvent
    {
        public delegate void Delegate(string checkboxEventName, bool value);
        static private event Delegate OnEvent;

        static public void Register(Delegate callback)
        {
            OnEvent += callback;
        }

        static public void Unregister(Delegate callback)
        {
            OnEvent -= callback;
        }

        static public void Trigger(string checkboxEventName, bool value)
        {
            OnEvent?.Invoke(checkboxEventName, value);
        }
    }
}