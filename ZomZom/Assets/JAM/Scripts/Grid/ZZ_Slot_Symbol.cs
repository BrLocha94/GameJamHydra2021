using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(SpriteRendererGroupMember))]
[ExecuteInEditMode]
public class ZZ_Slot_Symbol : MonoBehaviour
{
    [HideInInspector][SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector][SerializeField] private SpriteRendererGroupMember groupMember;

    private MaterialPropertyBlock propertyBlock;
    private int topColorID = Shader.PropertyToID("_TopColor");

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        groupMember = GetComponent<SpriteRendererGroupMember>();
    }

    public void SetSprite(Sprite sprite)=>spriteRenderer.sprite = sprite;
    public void SetColor(Color color)=>groupMember.color = color;
    public void SetOpacity(float opacity)=> groupMember.alpha = opacity;
    public void SetPosition(Vector3 position)=> transform.localPosition = position;

    public void SetTopColor(Color color)
    {
        if(propertyBlock==null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(topColorID,color);
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }


}
