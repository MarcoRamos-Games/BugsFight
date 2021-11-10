using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float hoverAmount;

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
        transform.position += new Vector3(0, 0, -.1f);
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
        transform.position += new Vector3(0, 0, .1f);
    }

}
