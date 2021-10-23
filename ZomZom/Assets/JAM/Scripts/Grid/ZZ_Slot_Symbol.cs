using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(SpriteRendererGroupMember))]
[ExecuteInEditMode]
public class ZZ_Slot_Symbol : MonoBehaviour
{
    [HideInInspector][SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector][SerializeField] private SpriteRendererGroupMember groupMember;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        groupMember = GetComponent<SpriteRendererGroupMember>();
    }

    public void SetSprite(Sprite sprite)=>spriteRenderer.sprite = sprite;
    public void SetColor(Color color)=>groupMember.color = color;
}
