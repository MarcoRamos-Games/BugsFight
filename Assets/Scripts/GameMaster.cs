using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;

    public void ResetTiles()
    {
        foreach(Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
