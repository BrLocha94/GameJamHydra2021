using UnityEngine;

[RequireComponent(typeof(SpriteRendererGroupMember),typeof(SpriteRendererGroupMember))]
public class SlotSymbol: MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRendererGroupMember groupMember;

    public Color color
    {
        get=> groupMember.color;
        set=> groupMember.color = value;
    }

    public Sprite sprite
    {
        get=> spriteRenderer.sprite;
        set=> spriteRenderer.sprite = value;
    }
}