using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    GameMaster gm;
    public float hoverAmount;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }
    private void OnMouseEnter()
    {
        //if (GetComponent<Unit>().playerNumber == gm.playerTurn)
        //{
        //    transform.localScale += Vector3.one * hoverAmount;
        //    transform.position += new Vector3(0, 0, -.1f);
        //}
        transform.localScale += Vector3.one * hoverAmount;
        transform.position += new Vector3(0, 0, -.1f);
    }

    private void OnMouseExit()
    {
        /*
        if (GetComponent<Unit>().playerNumber == gm.playerTurn)
        {
            transform.localScale -= Vector3.one * hoverAmount;
            transform.position += new Vector3(0, 0, .1f);
        }
        */
        transform.localScale -= Vector3.one * hoverAmount;
        transform.position += new Vector3(0, 0, .1f);

    }

}
