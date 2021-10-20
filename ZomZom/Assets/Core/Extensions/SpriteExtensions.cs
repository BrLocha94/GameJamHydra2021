using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteExtensions
{
    public static void SetAlpha(this SpriteRenderer render, float alpha)
    {
        var c = render.color;
        c.a = alpha;
        render.color = c;
    }
    public static void SetAlpha(this Graphic render, float alpha)
    {
        var c = render.color;
        c.a = alpha;
        render.color = c;
    }
   
}