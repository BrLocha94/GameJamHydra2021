using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReelMask : MonoBehaviour
{
    [SerializeField] private Reel reel;

    private void Update()
    {
        if(reel!=null)
        {
            transform.localScale = reel.bounds.extents;
            transform.localPosition = reel.bounds.center;
        }
    }
}
