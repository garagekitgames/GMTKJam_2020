using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MoreMountains.Tools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MMDebugMenuData))]
    public class MMDebugMenuDataEditor : Editor
    {
        protected ReorderableList _list;

        protected virtual void OnEnable()
        {
            _list = new ReorderableList(serializedObject.FindProperty("MenuItems"));
            _list.elementNameProperty = "MenuItems";
            _list.elementDisplayType = ReorderableList.ElementDisplayType.Expandable;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "MenuItems");

            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
