using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{
    //[REVISIT]: Check if practical applications of methods work as expected
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    public static SpriteRenderer AddSpriteRenderer(this GameObject obj, SpriteRenderer source)
    {
        var s = obj.AddComponent<SpriteRenderer>();
        s.sprite = source.sprite;
        s.sortingLayerName = source.sortingLayerName;
        s.sortingOrder = source.sortingOrder;
        //s.material = source.material;
        s.color = source.color;

        return s;
    }
    public static SpriteRenderer AddSpriteRenderer(this GameObject obj)
    {
        return obj.AddComponent<SpriteRenderer>();
    }
}
